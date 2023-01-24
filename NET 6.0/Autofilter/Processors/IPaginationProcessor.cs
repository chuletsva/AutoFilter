using Autofilter.Models;

namespace Autofilter.Processors;

internal interface IPaginationProcessor
{
    IQueryable<T> ApplyPagination<T>(IQueryable<T> source, PaginationRule pagination);
}

internal class PaginationProcessor : IPaginationProcessor
{
    public IQueryable<T> ApplyPagination<T>(IQueryable<T> source, PaginationRule pagination)
    {
        (int? skip, int? top) = pagination;

        if (skip is not null)
        {
            source = source.Skip(skip.Value);
        }

        if (top is not null)
        {
            source = source.Take(top.Value);
        }

        return source;
    }
}