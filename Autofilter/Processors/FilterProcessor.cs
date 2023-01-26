using Autofilter.Exceptions;

namespace Autofilter.Processors;

internal class FilterProcessor
{
    public static FilterProcessor Instance => _instance.Value;

    private static readonly Lazy<FilterProcessor> _instance = new Lazy<FilterProcessor>(
        () => new FilterProcessor(new SearchProcessor(), new SortingProcessor(), new PaginationProcessor()));

    private readonly ISearchProcessor _searchProcessor;
    private readonly ISortingProcessor _sortingProcessor;
    private readonly IPaginationProcessor _paginationProcessor;
    
    public FilterProcessor(ISearchProcessor searchProcessor, 
        ISortingProcessor sortingProcessor, 
        IPaginationProcessor paginationProcessor)
    {
        _searchProcessor = searchProcessor;
        _sortingProcessor = sortingProcessor;
        _paginationProcessor = paginationProcessor;
    }

    public IQueryable<T> ApplyFilter<T>(IQueryable<T> source, Filter filter)
    {
        try
        {
            if (filter.Search is not null)
            {
                source = _searchProcessor.ApplySearch(source, filter.Search, filter.Groups);
            }

            if (filter.Sorting is not null)
            {
                source = _sortingProcessor.ApplySorting(source, filter.Sorting);
            }

            if (filter.Pagination is not null)
            {
                source = _paginationProcessor.ApplyPagination(source, filter.Pagination);
            }

            return source;
        }
        catch (Exception ex)
        {
            throw new FilterException("Filtering error", ex);
        }
    }
}