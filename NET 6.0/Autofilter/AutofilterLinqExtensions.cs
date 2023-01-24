using Autofilter.Processors;

namespace Autofilter;

public static class AutofilterLinqExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, Filter filter)
    {
        return FilterProcessor.Instance.ApplyFilter(source, filter);
    }
}