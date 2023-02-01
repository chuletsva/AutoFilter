using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests;

public class AutofilterLinqExtensionsTests
{
    [Fact]
    public void ApplyFilter()
    {
        var matchObject = new TestClass();

        var queryable = new[] { matchObject, new() }.AsQueryable();

        var filter = new AutoFilter()
        {
            Filter = new FilterRule
            (
                Conditions: new[]
                {
                    new Condition(nameof(TestClass.Id), new [] { matchObject.Id.ToString() }, SearchOperator.Equals)
                }
            )
        };

        var resultQueryable = queryable.ApplyFilter(filter);

        resultQueryable.Should().ContainSingle(x => x == matchObject);
    }

    [Fact]
    public void ApplyFiltersAndSelect()
    {
        var matchObject = new TestClass() { Prop1 = true, Prop2 = true };

        var queryable = new[]
        {
            matchObject,
            new() { Prop1 = true, Prop2 = false },
            new() { Prop1 = false, Prop2 = false },
        }
        .AsQueryable();

        var filter1 = new AutoFilter()
        {
            Filter = new FilterRule
            (
                Conditions: new[]
                {
                    new Condition(nameof(TestClass.Prop1), new [] { matchObject.Prop1.ToString() }, SearchOperator.Equals)
                }
            )
        };

        var filter2 = new AutoFilter()
        {
            Filter = new FilterRule
            (
                Conditions: new[]
                {
                    new Condition(nameof(TestClass.Prop2), new [] { matchObject.Prop2.ToString() }, SearchOperator.Equals)
                }
            ),
            Select = new [] { nameof(TestClass.Id) }
        };

        var resultQueryable = (IQueryable<Dictionary<string, object>>) queryable.ApplyFiltersAndSelect(filter1, filter2);

        resultQueryable.Should().ContainSingle();

        resultQueryable.Single().Keys.Should().ContainSingle(nameof(TestClass.Id));

        resultQueryable.Single().Values.Should().ContainSingle(matchObject.Id.ToString());
    }

    private class TestClass
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public bool Prop1 { get; init; }

        public bool Prop2 { get; init; }
    }
}
