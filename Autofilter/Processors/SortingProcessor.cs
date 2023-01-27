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

        PropertyInfo property = Reflection.GetProperty(queryable.ElementType, propertyName);

        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");
        MemberExpression propExpr = Expression.Property(paramExpr, property);
        Expression keySelector = Expression.Lambda(propExpr, paramExpr);

        string sortingMethodName = (thenBy, isDescending) switch
        {
            (false, false) => "OrderBy",
            (true, false) => "ThenBy",
            (false, true) => "OrderByDescending",
            (true, true) => "ThenByDescending",
        };

        MethodInfo method = typeof(Queryable).GetMethods()
            .Single(x => x.Name == sortingMethodName && x.GetParameters().Length == 2)
            .MakeGenericMethod(queryable.ElementType, property.PropertyType);

        var sorted = method.Invoke(null, new object[]{ queryable, keySelector }) ?? throw new NullReferenceException("Unreachable exception");

        return (IQueryable) sorted;
    }
}