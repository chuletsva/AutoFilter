using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;
using Autofilter.Rules;

namespace Autofilter.Processors;

internal static class DistinctProcessor
{
    public static IQueryable ApplyDistinct(IQueryable queryable, DistinctRule distinct)
    {
        if (distinct.PropertyName is null)
        {
            var method = typeof(Queryable).GetMethods()
                .Single(x => x.Name == "Distinct" && x.GetParameters().Length is 1)
                .MakeGenericMethod(queryable.ElementType);

            var distinctQueryable = method.Invoke(null, new object?[] { queryable }) ?? throw new NullReferenceException();

            return (IQueryable) distinctQueryable;
        }
        else
        {
            PropertyInfo property = Reflection.GetProperty(queryable.ElementType, distinct.PropertyName);

            ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");
            MemberExpression propExpr = Expression.Property(paramExpr, property);
            Expression keySelector = Expression.Lambda(propExpr, paramExpr);

            var method = typeof(Queryable).GetMethods()
                .Single(x => x.Name == "DistinctBy" && x.GetParameters().Length == 2)
                .MakeGenericMethod(queryable.ElementType, property.PropertyType);

            var distinctQueryable = method.Invoke(null, new object?[] { queryable, keySelector }) ?? throw new NullReferenceException();

            return (IQueryable) distinctQueryable;
        }
    }
}
