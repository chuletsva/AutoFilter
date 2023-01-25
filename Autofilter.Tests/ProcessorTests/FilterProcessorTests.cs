using Autofilter.Processors;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace Autofilter.Tests.ProcessorTests;

public class FilterProcessorTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ISearchProcessor> _serachProcessor;
    private readonly Mock<ISortingProcessor> _sortingProcessor;
    private readonly Mock<IPaginationProcessor> _paginationProcessor;
    private readonly FilterProcessor _sut;

    public FilterProcessorTests()
    {
        _fixture = new Fixture();
        _serachProcessor = new Mock<ISearchProcessor>();
        _sortingProcessor = new Mock<ISortingProcessor>();
        _paginationProcessor = new Mock<IPaginationProcessor>();
        _sut = new FilterProcessor(_serachProcessor.Object, 
            _sortingProcessor.Object, _paginationProcessor.Object);
    }

    [Fact]
    public void ShouldReturnSameQuery_WhenDefaultFilter()
    {
        IQueryable<int> query = _fixture.CreateMany<int>().AsQueryable();

        IQueryable<int> resultQuery = _sut.ApplyFilter(query, new Filter());

        resultQuery.Should().BeSameAs(query);
    }

    [Fact]
    public void ShouldCallSearchProcessor()
    {
        IQueryable<int> query = _fixture.CreateMany<int>().AsQueryable();

        var filter = _fixture.Create<Filter>();

        _sut.ApplyFilter(query, filter);

        _serachProcessor.Verify(x => x.ApplySearch(It.IsAny<IQueryable<int>>(), filter.Search, filter.Groups), Times.Once);
    }

    [Fact]
    public void ShouldCallSortingProcessor()
    {
        IQueryable<int> query = _fixture.CreateMany<int>().AsQueryable();

        var filter = _fixture.Create<Filter>();

        _sut.ApplyFilter(query, filter);

        _sortingProcessor.Verify(x => x.ApplySorting(It.IsAny<IQueryable<int>>(), filter.Sorting), Times.Once);
    }

    [Fact]
    public void ShouldCallPaginationProcessor()
    {
        IQueryable<int> query = _fixture.CreateMany<int>().AsQueryable();

        var filter = _fixture.Create<Filter>();

        _sut.ApplyFilter(query, filter);

        _paginationProcessor.Verify(x => x.ApplyPagination(It.IsAny<IQueryable<int>>(), filter.Pagination), Times.Once);
    }
}