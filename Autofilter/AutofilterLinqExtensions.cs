using Autofilter.Exceptions;
using Autofilter.Processors;

namespace Autofilter;

public static class AutofilterLinqExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, AutoFilter filter) where T : class
    {
        if (filter.Select is { Length: >0 })
        {
            throw new FilterException("Select operation currently is not supported in this method");
        }

        return (IQueryable<T>) ApplyFilterAndSelect(queryable, filter);
    }

    public static IQueryable ApplyFilterAndSelect(this IQueryable queryable, AutoFilter filter)
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
                queryable = SelectProcessor.ApplySelectDictionary(queryable, filter.Select);
            }

            return queryable;
        }
        catch (Exception ex)
        {
            throw new FilterException("Error while applying filter", ex);
        }
    }

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> queryable, params AutoFilter[] filters) where T : class
    {
        if (filters.Any(x => x.Select is { Length: >0 }))
        {
            throw new FilterException("Select operation currently is not supported in this method");
        }

        return (IQueryable<T>) ApplyFiltersAndSelect(queryable, filters);
    }

    public static IQueryable ApplyFiltersAndSelect(this IQueryable queryable, params AutoFilter[] filters)
    {
        return filters.Aggregate(queryable, (query, filter) => query.ApplyFilterAndSelect(filter));
    }
}