using Autofilter.Model;

namespace Autofilter.Processors;

interface ISortingProcessor
{
    IQueryable<T> ApplySorting<T>(IQueryable<T> source, SortingRule[] sorting);
}