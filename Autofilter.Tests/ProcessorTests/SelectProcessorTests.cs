using Autofilter.Processors;
using AutoFixture;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests;

public class SelectProcessorTests
{
    [Fact]
    public void ShouldSelectProperties()
    {
        IQueryable query = new Fixture().CreateMany<TestClass>().AsQueryable();

        var properties = new[] { nameof(TestClass.Prop1) };

        query = SelectProcessor.ApplySelectDictionary(query, properties);

        query.ElementType.Should().Be(typeof(Dictionary<string, object>));
    }

    private class TestClass
    {
        public bool Prop1 { get; set; }
        public int Prop2 { get; set; }
    }
}
