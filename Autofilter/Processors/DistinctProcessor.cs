using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;

namespace Autofilter.Processors;

internal static class DistinctProcessor
{
    public static IQueryable ApplyDistinct(IQueryable queryable, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            var method = QueryableMethods.Distinct(queryable.ElementType);

            var distinctQueryable = method.Invoke(null, new object?[] { queryable }) ?? throw new NullReferenceException();

            return (IQueryable) distinctQueryable;
        }
        else
        {
            PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, propertyName);

            ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");
            MemberExpression propExpr = Expression.Property(paramExpr, property);
            Expression keySelector = Expression.Lambda(propExpr, paramExpr);

            var method = QueryableMethods.DistinctBy(queryable.ElementType, property.PropertyType);

            var distinctQueryable = method.Invoke(null, new object?[] { queryable, keySelector }) ?? throw new NullReferenceException();

            return (IQueryable) distinctQueryable;
        }
    }
}
