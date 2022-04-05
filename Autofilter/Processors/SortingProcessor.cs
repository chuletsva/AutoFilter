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
            ApplySortingRule(source, typeof(T), sorting.First()), 
            (acc, rule) => ApplySortingRule(acc, typeof(T), rule, true));
    }

    private static object ApplySortingRule(
        object source, Type elementType, 
        SortingRule rule, bool thenBy = false)
    {
        if (!Reflection.TryGetProperty(elementType, rule.PropertyName, out var property))
            throw new Exception($"Property '{rule.PropertyName}' of type '{elementType.Name}' doesn't exist");

        string methodName = (thenBy, rule.IsDescending) switch
        {
            (false, false) => "OrderBy",
            (true, false) => "ThenBy",
            (false, true) => "OrderByDescending",
            (true, true) => "ThenByDescending",
        };
        MethodInfo orderByMethod = typeof(Queryable).GetMethods().Single(x => x.Name == methodName && x.GetParameters().Length == 2);
        MethodInfo orderBy = orderByMethod.MakeGenericMethod(elementType, property.PropertyType);

        ParameterExpression paramExpr = Expression.Parameter(elementType, "x");
        MemberExpression propExpr = Expression.Property(paramExpr, property);
        Expression keySelector = Expression.Lambda(propExpr, paramExpr);

        return orderBy.Invoke(null, new[]{ source, keySelector})!;
    }
}