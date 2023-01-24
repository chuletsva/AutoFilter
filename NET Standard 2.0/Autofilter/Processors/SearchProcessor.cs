using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;

namespace Autofilter.Processors;

class SearchProcessor : ISearchProcessor
{
    public IQueryable<T> ApplySearch<T>(IQueryable<T> source, 
        SearchRule[] search, GroupRule[]? groups)
    {
        if (search.Length is 0) return source;

        Expression<Func<T, bool>> predicate = 
            PredicateBuilder.BuildPredicate<T>(search, groups);

        return source.Where(predicate);
    }
}