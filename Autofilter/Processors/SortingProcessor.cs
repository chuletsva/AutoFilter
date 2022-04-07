using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;
using Autofilter.Model;

namespace Autofilter.Processors;

class SortingProcessor : ISortingProcessor
{
    public IQueryable<T> ApplySorting<T>(IQueryable<T> source, SortingRule[] sorting)
    {
        if (sorting.Length is 0) return source;

        return (IQueryable<T>) sorting.Skip(1).Aggregate(
            ApplySortingRule(source, typeof(T), sorting[0]), 
            (query, rule) => ApplySortingRule(query, typeof(T), rule, true));
    }

    private static object ApplySortingRule(
        object queryable, Type elementType, 
        SortingRule rule, bool thenBy = false)
    {
        (string propertyName, bool isDescending) = rule;

        PropertyInfo property = Reflection.GetProperty(elementType, propertyName);

        string methodName = (thenBy, isDescending) switch
        {
            (false, false) => "OrderBy",
            (true, false) => "ThenBy",
            (false, true) => "OrderByDescending",
            (true, true) => "ThenByDescending",
        };

        MethodInfo orderBy = typeof(Queryable).GetMethods()
            .Single(x => x.Name == methodName && x.GetParameters().Length == 2)
            .MakeGenericMethod(elementType, property.PropertyType);

        ParameterExpression paramExpr = Expression.Parameter(elementType, "x");
        MemberExpression propExpr = Expression.Property(paramExpr, property);
        Expression keySelector = Expression.Lambda(propExpr, paramExpr);

        return orderBy.Invoke(null, new[]{ queryable, keySelector })!;
    }
}