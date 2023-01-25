using Autofilter.Processors;

namespace Autofilter;

public static class AutofilterLinqExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, Filter filter) where T : class
    {
        return FilterProcessor.Instance.ApplyFilter(source, filter);
    }
}