using Autofilter.Models;
using Autofilter.Processors;
using AutoFixture;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class SortingProcessorTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ShouldReturnSameQuery_WhenNoRules()
    {
        var query = _fixture.CreateMany<TestClass>().AsQueryable();

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, Array.Empty<SortingRule>());

        actualQuery.Should().BeSameAs(query);
    }

    [Fact]
    public void ShouldSortByAscending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        SortingRule[] sorting = { new(nameof(TestClass.Prop1)) };

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, sorting);

        actualQuery.Should().BeInAscendingOrder(x => x.Prop1);
    }

    [Fact]
    public void ShouldSortByAscending_ThenByAscending()
    {
        SortingRule[] sorting =
        {
            new(nameof(TestClass.Prop1)), 
            new(nameof(TestClass.Prop2))
        };

        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, sorting);

        actualQuery.Should().BeInAscendingOrder(x => x.Prop1).And.ThenBeInAscendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldSortByAscending_ThenByDescending()
    {
        SortingRule[] sorting =
        {
            new(nameof(TestClass.Prop1)), 
            new(nameof(TestClass.Prop2), true)
        };

        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, sorting);

        actualQuery.Should().BeInAscendingOrder(x => x.Prop1).And.ThenBeInDescendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldSortByDescending()
    {
        SortingRule[] sorting = { new(nameof(TestClass.Prop1), true) };

        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, sorting);

        actualQuery.Should().BeInDescendingOrder(x => x.Prop1);
    }

    [Fact]
    public void ShouldSortByDescending_ThenByAscending()
    {
        SortingRule[] sorting =
        {
            new(nameof(TestClass.Prop1), true), 
            new(nameof(TestClass.Prop2))
        };

        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, sorting);

        actualQuery.Should().BeInDescendingOrder(x => x.Prop1).And.ThenBeInAscendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldSortByDescending_ThenByDescending()
    {
        SortingRule[] sorting =
        {
            new(nameof(TestClass.Prop1), true), 
            new(nameof(TestClass.Prop2), true)
        };

        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var sut = new SortingProcessor();

        var actualQuery = sut.ApplySorting(query, sorting);

        actualQuery.Should().BeInDescendingOrder(x => x.Prop1).And.ThenBeInDescendingOrder(x => x.Prop2);
    }

    [Fact]
    public void ShouldThrow_WhenInvalidProperty()
    {
        var query = Array.Empty<TestClass>().AsQueryable();

        SortingRule[] sorting = { new("") };

        var sut = new SortingProcessor();

        FluentActions.Invoking(() => sut.ApplySorting(query, sorting))
            .Should().Throw<Exception>().WithMessage($"Property '' for type '{nameof(TestClass)}' doesn't exist");
    }

    private class TestClass
    {
        public int Prop1 { get; init; }
        public int Prop2 { get; init; }
    }
}