using System.Runtime.CompilerServices;
using Autofilter.Processors;

[assembly: InternalsVisibleTo("Autofilter.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Autofilter;

public static class LinqExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, Filter filter)
    {
        return FilterProcessor.Instance.ApplyFilter(source, filter);
    }
}