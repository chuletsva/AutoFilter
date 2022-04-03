using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;

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

    public ComposeOperator? Compose => _rule.ComposeOperator;

    public Expression BuildExpression()
    {
        if (!Reflection.TryGetProperty(_paramExpr.Type, _rule.PropertyName, out var property))
            throw new Exception($"Property '{_rule.PropertyName}' of type '{_paramExpr.Type.Name}' doesn't exist");

        MemberExpression propExpr = Expression.Property(_paramExpr, property);

        Expression valueExpr;

        try
        {
            object? value = ValueConverter.ConvertValueToType(_rule.Value, property.PropertyType);
            valueExpr = Expression.Constant(value, property.PropertyType);
        }
        catch
        {
            throw new Exception($"Cannot convert {_rule.Value} to type '{property.PropertyType}'");
        }

        return _rule.SearchOperator switch
        {
            SearchOperator.Equals => Expression.Equal(propExpr, valueExpr),

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

            _ => throw new ArgumentOutOfRangeException(nameof(_rule.SearchOperator))
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