using System;
using System.Linq;
using Autofilter.Model;
using Autofilter.Processors;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.Processors;

public class SortingProcessorTests
{
    class TestClass
    {
        public int P1 { get; init; }
        public int P2 { get; init; }
    }

    private readonly IQueryable<TestClass> _query = new TestClass[]
    {
        new(){ P1 = 3, P2 = 1 },
        new(){ P1 = 1, P2 = 3 },
        new(){ P1 = 2, P2 = 4 },
        new(){ P1 = 3, P2 = 2 },
        new(){ P1 = 1, P2 = 1 },
        new(){ P1 = 2, P2 = 6 },
        new(){ P1 = 3, P2 = 3 },
        new(){ P1 = 1, P2 = 2 },
        new(){ P1 = 2, P2 = 5 },
    }.AsQueryable();

    [Fact]
    public void ReturnsSameQuery_WhenZeroSortingRulesPassed()
    {
        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, Array.Empty<SortingRule>());

        resultQuery.Should().BeSameAs(_query);
    }

    [Fact]
    public void SortsByAscending()
    {
        SortingRule[] rules = { new(nameof(TestClass.P1)) };

        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, rules);

        resultQuery.Should().BeInAscendingOrder(x => x.P1);
    }
    
    [Fact]
    public void SortsByAscending_ThenByAscending()
    {
        SortingRule[] rules =
        {
            new(nameof(TestClass.P1)), 
            new(nameof(TestClass.P2))
        };

        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, rules);

        resultQuery.Should().BeInAscendingOrder(x => x.P1)
            .And.ThenBeInAscendingOrder(x => x.P2);
    }

    [Fact]
    public void SortsByAscending_ThenByDescending()
    {
        SortingRule[] rules =
        {
            new(nameof(TestClass.P1)), 
            new(nameof(TestClass.P2), true)
        };

        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, rules);

        resultQuery.Should().BeInAscendingOrder(x => x.P1)
            .And.ThenBeInDescendingOrder(x => x.P2);
    }

    [Fact]
    public void SortsByDescending()
    {
        SortingRule[] rules = { new(nameof(TestClass.P1), true) };

        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, rules);

        resultQuery.Should().BeInDescendingOrder(x => x.P1);
    }

    [Fact]
    public void SortsByDescending_ThenByAscending()
    {
        SortingRule[] rules =
        {
            new(nameof(TestClass.P1), true), 
            new(nameof(TestClass.P2))
        };

        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, rules);

        resultQuery.Should().BeInDescendingOrder(x => x.P1)
            .And.ThenBeInAscendingOrder(x => x.P2);
    }

    [Fact]
    public void SortsByDescending_ThenByDescending()
    {
        SortingRule[] rules =
        {
            new(nameof(TestClass.P1), true), 
            new(nameof(TestClass.P2), true)
        };

        IQueryable<TestClass> resultQuery = new SortingProcessor()
            .ApplySorting(_query, rules);

        resultQuery.Should().BeInDescendingOrder(x => x.P1)
            .And.ThenBeInDescendingOrder(x => x.P2);
    }

    [Fact]
    public void Throws_WhenInvalidProperty()
    {
        IQueryable<TestClass> query = Array.Empty<TestClass>().AsQueryable();

        SortingRule[] rules = { new("") };

        Action act = () => new SortingProcessor().ApplySorting(query, rules);

        act.Should().Throw<Exception>().WithMessage($"Property '' of type '{nameof(TestClass)}' doesn't exist");
    }
}