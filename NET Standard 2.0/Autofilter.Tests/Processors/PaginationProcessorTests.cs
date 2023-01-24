using System.Linq;
using Autofilter.Processors;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.Processors;

public class PaginationProcessorTests
{

    [Fact]
    public void ReturnsSameQuery_WhenRuleIsEmpty()
    {
        IQueryable<int> sourceQuery = new Fixture()
            .CreateMany<int>(1000).AsQueryable();

        IQueryable<int> resultQuery = new PaginationProcessor()
            .ApplyPagination(sourceQuery, new());

        resultQuery.Should().BeSameAs(sourceQuery);
    }

    [Fact]
    public void SkipsElements()
    {
        IQueryable<int> sourceQuery = new Fixture()
            .CreateMany<int>(1000).AsQueryable();

        IQueryable<int> expectedQuery = sourceQuery.Skip(500);

        IQueryable<int> resultQuery = new PaginationProcessor()
            .ApplyPagination(sourceQuery, new(Skip: 500));

        resultQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void TakesElements()
    {
        IQueryable<int> sourceQuery = new Fixture()
            .CreateMany<int>(1000).AsQueryable();

        IQueryable<int> expectedQuery = sourceQuery.Take(500);

        IQueryable<int> resultQuery = new PaginationProcessor()
            .ApplyPagination(sourceQuery, new(Top: 500));

        resultQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }
}