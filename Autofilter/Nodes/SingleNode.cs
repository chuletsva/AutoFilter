using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;
using Autofilter.Models;
using static Autofilter.Helpers.ValueConverter;

namespace Autofilter.Nodes;

internal class SingleNode : INode
{
    private readonly SearchRule _rule;
    private readonly ParameterExpression _paramExpr;

    public SingleNode(SearchRule rule, ParameterExpression paramExpr)
    {
        _rule = rule;
        _paramExpr = paramExpr;
    }

    public LogicOperator? Operator => _rule.LogicOperator;

    public Expression BuildExpression()
    {
        PropertyInfo property = Reflection.GetProperty(_paramExpr.Type, _rule.Name);

        MemberExpression propExpr = Expression.Property(_paramExpr, property);

        object? value = ConvertValueToType(_rule.Value, property.PropertyType);

        Expression valueExpr;
        try
        {
            valueExpr = Expression.Constant(value, property.PropertyType);
        }
        catch
        {
            throw new Exception($"Property '{_rule.Name}' of type '{property.PropertyType.Name}' is not comparable with {GetInvalidValueAlias(value)}");
        }

        Expression predicateExpr = _rule.SearchOperator switch
        {
            SearchOperator.Equals when property.PropertyType == typeof(bool) => value switch
            {
                true => propExpr,
                false => Expression.Not(propExpr),
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            },

            SearchOperator.Equals => Expression.Equal(propExpr, valueExpr),

            SearchOperator.NotEquals when property.PropertyType == typeof(bool) => value switch
            {
                true => Expression.Not(propExpr),
                false => propExpr,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            },

            SearchOperator.NotEquals => Expression.NotEqual(propExpr, valueExpr),

            SearchOperator.Greater when Reflection.IsComparable(property.PropertyType) 
                => Expression.GreaterThan(propExpr, valueExpr),

            SearchOperator.GreaterOrEqual when Reflection.IsComparable(property.PropertyType) 
                => Expression.GreaterThanOrEqual(propExpr, valueExpr),

            SearchOperator.Less when Reflection.IsComparable(property.PropertyType)
                => Expression.LessThan(propExpr, valueExpr),

            SearchOperator.LessOrEqual when Reflection.IsComparable(property.PropertyType)
                => Expression.LessThanOrEqual(propExpr, valueExpr),

            SearchOperator.Exists when Reflection.CanBeNull(property.PropertyType)
                => Expression.NotEqual(propExpr, Expression.Constant(null)),

            SearchOperator.NotExists when Reflection.CanBeNull(property.PropertyType)
                => Expression.Equal(propExpr, Expression.Constant(null)),

            SearchOperator.StartsWith when property.PropertyType == typeof(string)
                => Expression.Call(propExpr, "StartsWith", null, valueExpr),

            SearchOperator.EndsWith when property.PropertyType == typeof(string)
                => Expression.Call(propExpr, "EndsWith", null, valueExpr),

            SearchOperator.Contains when property.PropertyType == typeof(string)
                => Expression.Call(propExpr, "Contains", null, valueExpr),

            SearchOperator.NotContains when property.PropertyType == typeof(string)
                => Expression.Not(Expression.Call(propExpr, "Contains", null, valueExpr)),

            _ => throw new Exception($"Operator '{_rule.SearchOperator}' is not supported for type '{property.PropertyType.Name}'")
        };

        return predicateExpr;
    }
}