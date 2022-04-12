using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;
using Autofilter.Model;
using static Autofilter.Helpers.ValueConverter;

namespace Autofilter.Nodes;

class SingleNode : INode
{
    private readonly SearchRule _rule;
    private readonly ParameterExpression _paramExpr;

    public SingleNode(SearchRule rule, ParameterExpression paramExpr)
    {
        _rule = rule;
        _paramExpr = paramExpr;
    }

    public LogicOperator? Logic => _rule.LogicOperator;

    public Expression BuildExpression()
    {
        PropertyInfo property = Reflection.GetProperty(_paramExpr.Type, _rule.PropertyName);

        MemberExpression propExpr = Expression.Property(_paramExpr, property);

        object? value = ConvertValueToType(_rule.Value, property.PropertyType);

        Expression valueExpr;
        try { valueExpr = Expression.Constant(value, property.PropertyType); }
        catch
        {
            throw new Exception($"Property '{_rule.PropertyName}' with type '{property.PropertyType.Name}' is not comparable with {GetInvalidValueAlias(value)}");
        }

        return _rule.SearchOperator switch
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

            SearchOperator.Greater when Reflection.IsComparableType(property.PropertyType) 
                => Expression.GreaterThan(propExpr, valueExpr),

            SearchOperator.GreaterOrEqual when Reflection.IsComparableType(property.PropertyType) 
                => Expression.GreaterThanOrEqual(propExpr, valueExpr),

            SearchOperator.Less when Reflection.IsComparableType(property.PropertyType)
                => Expression.LessThan(propExpr, valueExpr),

            SearchOperator.LessOrEqual when Reflection.IsComparableType(property.PropertyType)
                => Expression.LessThanOrEqual(propExpr, valueExpr),

            SearchOperator.Exists when Reflection.IsNullableType(property.PropertyType)
                => Expression.NotEqual(propExpr, Expression.Constant(null)),

            SearchOperator.NotExists when Reflection.IsNullableType(property.PropertyType)
                => Expression.Equal(propExpr, Expression.Constant(null)),

            SearchOperator.StartsWith when property.PropertyType == typeof(string)
                => BuildStringMethodCall(propExpr, "StartsWith", valueExpr),

            SearchOperator.EndsWith when property.PropertyType == typeof(string)
                => BuildStringMethodCall(propExpr, "EndsWith", valueExpr),

            SearchOperator.Contains when property.PropertyType == typeof(string)
                => BuildStringMethodCall(propExpr, "Contains", valueExpr),

            SearchOperator.NotContains when property.PropertyType == typeof(string)
                => BuildNotContainsCall(propExpr, valueExpr),

            _ => throw new Exception($"Operation '{_rule.SearchOperator}' is not supported for type '{property.PropertyType.Name}'")
        };
    }

    private static Expression BuildStringMethodCall(
        Expression property, string methodName, Expression value)
    {
        // property != null && value != null && property.Method(value)
        return Expression.AndAlso(
            Expression.AndAlso(
                Expression.NotEqual(property, Expression.Constant(null)),
                Expression.NotEqual(value, Expression.Constant(null))),
            Expression.Call(property, methodName, null, value));
    }

    private static Expression BuildNotContainsCall(
        Expression property, Expression value)
    {
        // property != null && value != null && !property.Contains(value)
        return Expression.AndAlso(
            Expression.AndAlso(
                Expression.NotEqual(property, Expression.Constant(null)),
                Expression.NotEqual(value, Expression.Constant(null))),
            Expression.Not(Expression.Call(property, "Contains", null, value)));
    }
}