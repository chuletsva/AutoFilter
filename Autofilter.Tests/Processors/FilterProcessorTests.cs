using System.Linq;
using Autofilter.Model;
using Autofilter.Processors;
using AutoFixture;
using Moq;
using Xunit;

namespace Autofilter.Tests.Processors;

public class FilterProcessorTests
{
    private readonly Mock<ISearchProcessor> _serachProcessor;
    private readonly Mock<ISortingProcessor> _sortingProcessor;
    private readonly Mock<IPaginationProcessor> _paginationProcessor;
    private readonly FilterProcessor _sut;

    public FilterProcessorTests()
    {
        _serachProcessor = new Mock<ISearchProcessor>();
        _sortingProcessor = new Mock<ISortingProcessor>();
        _paginationProcessor = new Mock<IPaginationProcessor>();
        _sut = new FilterProcessor(_serachProcessor.Object, 
            _sortingProcessor.Object, _paginationProcessor.Object);
    }

    [Fact]
    public void ShouldCallSearchProcessor()
    {
        Fixture fixture = new();

        IQueryable<int> query = fixture.CreateMany<int>().AsQueryable();

        SearchRule[] search = fixture.CreateMany<SearchRule>().ToArray();

        GroupRule[] groups = fixture.CreateMany<GroupRule>().ToArray();

        Filter filter = new() { Search = search, Groups = groups };

        _sut.ApplyFilter(query, filter);

        _serachProcessor.Verify(x => x.ApplySearch(query, search, groups), Times.Once);
    }

    [Fact]
    public void ShouldCallSortingProcessor()
    {
        Fixture fixture = new();

        IQueryable<int> query = fixture.CreateMany<int>().AsQueryable();

        SortingRule[] sorting = fixture.CreateMany<SortingRule>().ToArray();

        _sut.ApplyFilter(query, new() { Sorting = sorting });

        _sortingProcessor.Verify(x => x.ApplySorting(query, sorting), Times.Once);
    }

    [Fact]
    public void ShouldCallPaginationProcessor()
    {
        Fixture fixture = new();

        IQueryable<int> query = fixture.CreateMany<int>().AsQueryable();

        var pagination = fixture.Create<PaginationRule>();

        _sut.ApplyFilter(query, new() { Pagination = pagination });

        _paginationProcessor.Verify(x => x.ApplyPagination(query, pagination), Times.Once);
    }
}