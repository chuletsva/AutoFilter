using Autofilter.Helpers;
using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Rules;

namespace Autofilter.Processors;

internal static class SortingProcessor 
{
    public static IQueryable ApplySorting(IQueryable queryable, SortingRule sorting)
    {
        var (propertyName, thenBy, isDescending) = sorting;

        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, propertyName);

        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");
        MemberExpression propExpr = Expression.Property(paramExpr, property);
        Expression keySelector = Expression.Lambda(propExpr, paramExpr);

        var method = (thenBy, isDescending) switch
        {
            (false, false) => QueryableMethods.OrderBy(queryable.ElementType, property.PropertyType),
            (false, true) => QueryableMethods.OrderByDescending(queryable.ElementType, property.PropertyType),
            (true, false) => QueryableMethods.ThenBy(queryable.ElementType, property.PropertyType),
            (true, true) => QueryableMethods.ThenByDescending(queryable.ElementType, property.PropertyType),
        };

        var sortedQueryable = method.Invoke(null, new object[]{ queryable, keySelector }) ?? throw new NullReferenceException();

        return (IQueryable) sortedQueryable;
    }
}