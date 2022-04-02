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
        if (!Reflection.TryGetProperty<T>(_rule.PropertyName, out var property))
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
            SearchOperator.Greater when Reflection.IsComparableType(property.PropertyType) 
                => Expression.GreaterThan(propExpr, valueExpr),
            SearchOperator.Exists when Reflection.IsNullableType(property.PropertyType)
                => Expression.NotEqual(propExpr, Expression.Constant(null)),
            _ => throw new ArgumentOutOfRangeException(nameof(_rule.SearchOperator))
        };
    }

    /* Long Int Short Decimal Double Float DateTime Char Byte Enum */
}