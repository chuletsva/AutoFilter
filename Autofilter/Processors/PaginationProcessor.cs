using Autofilter.Model;

namespace Autofilter.Processors;

class PaginationProcessor : IPaginationProcessor
{
    public IQueryable<T> ApplyPagination<T>(IQueryable<T> source, PaginationRule pagination)
    {
        (int? skip, int? top) = pagination;

        if (skip is not null)
            source = source.Skip(skip.Value);

        if (top is not null)
            source = source.Take(top.Value);

        return source;
    }
}