using Autofilter.Helpers;
using System.Linq.Expressions;
using Autofilter.Models;

namespace Autofilter.Processors;

internal interface ISearchProcessor
{
    IQueryable<T> ApplySearch<T>(IQueryable<T> source, SearchRule[] search, GroupRule[]? groups);
}

internal class SearchProcessor : ISearchProcessor
{
    public IQueryable<T> ApplySearch<T>(IQueryable<T> source, SearchRule[] search, GroupRule[]? groups)
    {
        if (search.Length is 0) return source;

        Expression<Func<T, bool>> predicate = PredicateBuilder.Build<T>(search, groups);

        return source.Where(predicate);
    }
}