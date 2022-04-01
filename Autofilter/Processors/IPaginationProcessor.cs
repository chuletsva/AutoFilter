using Autofilter.Model;

namespace Autofilter.Processors;

interface IPaginationProcessor
{
    IQueryable<T> ApplyPagination<T>(IQueryable<T> source, PaginationRule pagination);
}