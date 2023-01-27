using Autofilter.Processors;
using AutoFixture;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class PaginationProcessorTests
{
    [Fact]
    public void ShouldSkip()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Skip(500);

        IQueryable actualQuery = PaginationProcessor.ApplySkip(sourceQuery, 500);

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldTake()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Take(500);

        var actualQuery = PaginationProcessor.ApplyTop(sourceQuery, 500);

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }
}