using Autofilter.Model;

namespace Autofilter.Processors;

class SortingProcessor : ISortingProcessor
{
    public IQueryable<T> ApplySorting<T>(IQueryable<T> source, SortingRule[] sorting)
    {
        throw new NotImplementedException();
    }
}