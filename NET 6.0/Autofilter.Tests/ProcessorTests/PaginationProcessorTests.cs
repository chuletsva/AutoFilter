using Autofilter.Models;
using Autofilter.Processors;
using AutoFixture;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class PaginationProcessorTests
{
    [Fact]
    public void ShouldReturnSameQuery_WhenDefaultRule()
    {
        var query = new Fixture().CreateMany<int>(1000).AsQueryable();

        var sut = new PaginationProcessor();

        IQueryable<int> actualQuery = sut.ApplyPagination(query, new PaginationRule());

        actualQuery.Should().BeSameAs(query);
    }

    [Fact]
    public void ShouldSkip()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Skip(500);

        var sut = new PaginationProcessor();

        IQueryable<int> actualQuery = sut.ApplyPagination(sourceQuery, new PaginationRule(Skip: 500));

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldTake()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Take(500);

        var sut = new PaginationProcessor();

        var actualQuery = sut.ApplyPagination(sourceQuery, new PaginationRule(Top: 500));

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }
}