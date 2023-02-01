using Autofilter.Processors;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class DistinctProcessorTests
{
    [Fact]
    public void ShouldReturnUniqueValues()
    {
        var obj = new TestClass();

        var queryable = new[] { obj, obj }.AsQueryable();

        queryable = (IQueryable<TestClass>)DistinctProcessor.ApplyDistinct(queryable, "");

        queryable.Should().HaveCount(1);
    }

    [Fact]
    public void ShouldReturnUniqueValuesByProperty()
    {
        var queryable = new TestClass[]
        {
            new() { Prop = true },
            new() { Prop = true },
            new() { Prop = false }
        }.AsQueryable();

        queryable = (IQueryable<TestClass>)DistinctProcessor.ApplyDistinct(queryable, nameof(TestClass.Prop));

        queryable.Should().HaveCount(2);
    }

    private class TestClass
    {
        public bool Prop { get; init; }
    }
}
