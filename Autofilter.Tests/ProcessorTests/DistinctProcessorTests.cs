using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class DistinctProcessorTests
{
    [Fact]
    public void ShouldReturnDistinctValues()
    {
        var obj = new TestClass();

        var queryable = new[] { obj, obj }.AsQueryable();

        DistinctRule distinct = new();

        queryable = (IQueryable<TestClass>)DistinctProcessor.ApplyDistinct(queryable, distinct);

        queryable.Should().HaveCount(1);
    }

    [Fact]
    public void ShouldReturnDistinctValuesByProperty()
    {
        var queryable = new TestClass[]
        {
            new() { Prop = true },
            new() { Prop = true },
            new() { Prop = false }
        }.AsQueryable();

        DistinctRule distinct = new(nameof(TestClass.Prop));

        queryable = (IQueryable<TestClass>)DistinctProcessor.ApplyDistinct(queryable, distinct);

        queryable.Should().HaveCount(2);
    }

    private class TestClass
    {
        public bool Prop { get; init; }
    }
}
