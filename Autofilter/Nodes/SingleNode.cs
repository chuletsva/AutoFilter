using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;

namespace Autofilter.Nodes;

class SingleNode<T> : INode
{
    private readonly SearchRule _rule;
    private readonly ParameterExpression _parameterExpr;

    public SingleNode(SearchRule rule, ParameterExpression parameterExpr)
    {
        _rule = rule;
        _parameterExpr = parameterExpr;
    }

    public ComposeOperator? Compose => _rule.ComposeOperator;

    public Expression BuildExpression()
    {
        if (!FastTypeInfo<T>.TryGetProperty(_rule.PropertyName, out var property))
            throw new Exception($"Property '{_rule.PropertyName}' of '{typeof(T).Name}' doesn't exist");

        MemberExpression propExpr = Expression.Property(_parameterExpr, property);

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
            //SearchOperator.Greater => Expression.GreaterThan(propExpr, valueExpr),
            //SearchOperator.Less => Expression.LessThan(propExpr, valueExpr),
            //SearchOperator.GreaterOrEqual => Expression.GreaterThanOrEqual(propExpr, valueExpr),
            //SearchOperator.LessOrEqual => Expression.LessThanOrEqual(propExpr, valueExpr),
            //SearchOperator.StartsWith => Expression.Equal(propExpr, valueExpr)
            //SearchOperator.Contains => Expression.Call(),
            //SearchOperator.NotContains => Expression.Equal(propExpr, valueExpr),
            _ => throw new ArgumentOutOfRangeException(nameof(_rule.SearchOperator))
        };
    }
}