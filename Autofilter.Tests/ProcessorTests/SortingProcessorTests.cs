using Autofilter.Processors;
using Autofilter.Rules;
using AutoFixture;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class SortingProcessorTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ShouldSortByAscending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule sorting = new(nameof(TestClass.Prop1));

        query = (IQueryable<TestClass>)SortingProcessor.ApplySorting(query, sorting);

        query.Should().BeInAscendingOrder(x => x.Prop1);
    }

    [Fact]
    public void ShouldSortByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule sorting = new(nameof(TestClass.Prop1))
        {
            IsDescending = true
        };

        query = (IQueryable<TestClass>)SortingProcessor.ApplySorting(query, sorting);

        query.Should().BeInDescendingOrder(x => x.Prop1);
    }

    [Fact]
    public void ShouldSortByAscending_ThenByAscending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule sorting = new(nameof(TestClass.Prop1));

        query = (IQueryable<TestClass>)SortingProcessor.ApplySorting(query, sorting);

        sorting = new SortingRule(nameof(TestClass.Prop2))
        {
            ThenBy = true
        };

        query = (IQueryable<TestClass>) SortingProcessor.ApplySorting(query, sorting);

        query.Should().BeInAscendingOrder(x => x.Prop1).And.ThenBeInAscendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldSortByAscending_ThenByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule sorting = new(nameof(TestClass.Prop1));

        query = (IQueryable<TestClass>)SortingProcessor.ApplySorting(query, sorting);

        sorting = new SortingRule(nameof(TestClass.Prop2))
        {
            ThenBy = true,
            IsDescending = true
        };

        query = (IQueryable<TestClass>) SortingProcessor.ApplySorting(query, sorting);

        query.Should().BeInAscendingOrder(x => x.Prop1).And.ThenBeInDescendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldSortByDescending_ThenByAscending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule sorting = new(nameof(TestClass.Prop1))
        {
            IsDescending = true
        };

        query = (IQueryable<TestClass>)SortingProcessor.ApplySorting(query, sorting);

        sorting = new SortingRule(nameof(TestClass.Prop2))
        {
            ThenBy = true
        };

        query = (IQueryable<TestClass>) SortingProcessor.ApplySorting(query, sorting);

        query.Should().BeInDescendingOrder(x => x.Prop1).And.ThenBeInAscendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldSortByDescending_ThenByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule sorting = new(nameof(TestClass.Prop1))
        {
            IsDescending = true
        };

        query = (IQueryable<TestClass>)SortingProcessor.ApplySorting(query, sorting);

        sorting = new SortingRule(nameof(TestClass.Prop2))
        {
            ThenBy = true,
            IsDescending = true
        };

        query = (IQueryable<TestClass>) SortingProcessor.ApplySorting(query, sorting);

        query.Should().BeInDescendingOrder(x => x.Prop1).And.ThenBeInDescendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldThrow_WhenInvalidProperty()
    {
        var query = Array.Empty<TestClass>().AsQueryable();

        SortingRule sorting = new("");

        FluentActions.Invoking(() => SortingProcessor.ApplySorting(query, sorting))
            .Should().Throw<Exception>().WithMessage("Property '' not found");
    }

    private class TestClass
    {
        public int Prop1 { get; init; }
        public int Prop2 { get; init; }
    }
}