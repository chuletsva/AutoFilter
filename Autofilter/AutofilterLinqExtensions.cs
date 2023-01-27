using Autofilter.Exceptions;
using Autofilter.Processors;

namespace Autofilter;

public static class AutofilterLinqExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, AutoFilter filter) where T : class
    {
        if (filter.Select is not null)
        {
            throw new FilterException("Select operation currently is not supported in this method");
        }

        return (IQueryable<T>) ApplyFilterDynamic(queryable, filter);
    }

    public static IQueryable ApplyFilterDynamic(this IQueryable queryable, AutoFilter filter)
    {
        try
        {
            if (filter.Filter is { Conditions.Length: >0 })
            {
                queryable = FilterProcessor.ApplyFilter(queryable, filter.Filter);
            }

            if (filter.Distinct is not null)
            {
                queryable = DistinctProcessor.ApplyDistinct(queryable, filter.Distinct);
            }

            if (filter.Sorting is not null)
            {
                queryable = SortingProcessor.ApplySorting(queryable, filter.Sorting);
            }

            if (filter.Skip is not null)
            {
                queryable = PaginationProcessor.ApplySkip(queryable, filter.Skip.Value);
            }

            if (filter.Top is not null)
            {
                queryable = PaginationProcessor.ApplyTop(queryable, filter.Top.Value);
            }

            if (filter.Select is { Length: >0 })
            {
                queryable = SelectProcessor.ApplySelect(queryable, filter.Select);
            }

            return queryable;
        }
        catch (Exception ex)
        {
            throw new FilterException("Error while applying filter", ex);
        }
    }
}