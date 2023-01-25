using Autofilter.Helpers;
using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Models;

namespace Autofilter.Processors;

internal interface ISortingProcessor
{
    IQueryable<T> ApplySorting<T>(IQueryable<T> source, SortingRule[] sorting);
}

internal class SortingProcessor : ISortingProcessor
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
        SortingRule sorting, bool thenBy = false)
    {
        (string name, bool isDescending) = sorting;

        PropertyInfo property = Reflection.GetProperty(elementType, name);

        string sortingMethodName = (thenBy, isDescending) switch
        {
            (false, false) => "OrderBy",
            (true, false) => "ThenBy",
            (false, true) => "OrderByDescending",
            (true, true) => "ThenByDescending",
        };

        MethodInfo sortingMethod = typeof(Queryable).GetMethods()
            .Single(x => x.Name == sortingMethodName && x.GetParameters().Length == 2)
            .MakeGenericMethod(elementType, property.PropertyType);

        ParameterExpression paramExpr = Expression.Parameter(elementType, "x");
        MemberExpression propExpr = Expression.Property(paramExpr, property);
        Expression keySelector = Expression.Lambda(propExpr, paramExpr);

        var sortedQueryable = sortingMethod.Invoke(null, new[]{ queryable, keySelector });

        return sortedQueryable ?? throw new NullReferenceException("Unreachable exception");
    }
}