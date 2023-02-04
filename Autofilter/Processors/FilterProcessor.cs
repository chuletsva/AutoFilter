using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Rules;

namespace Autofilter.Processors;

internal static class FilterProcessor
{
    public static IQueryable ApplyFilter(IQueryable queryable, FilterRule filter)
    {
        LambdaExpression lambdaExpr = PredicateBuilder.BuildPredicate(queryable.ElementType, filter.Conditions, filter.Groups);

        var method = QueryableMethods.Where(queryable.ElementType);

        var filteredQueryable = method.Invoke(null, new object[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable) filteredQueryable;
    }
}