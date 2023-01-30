using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests.Types;

public class BoolTests
{
    [Theory]
    [MemberData(nameof(BoolTestCases))]
    public void ShouldHandleBool(bool objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Bool = objValue };

        Condition condition = new(nameof(obj.Bool), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(BoolTestCases))]
    [MemberData(nameof(NullableBoolTestCases))]
    public void ShouldHandleNullableBool(bool? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableBool = objValue };

        Condition condition = new(nameof(obj.NullableBool), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldThrow_WhenNotComparableValue(string? searchValue)
    {
        Condition condition = new(nameof(TestClass.Bool), new[] { searchValue }, SearchOperator.Equals);

        FluentActions.Invoking(() => FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition }))
            .Should().Throw<Exception>().Which.Message
            .Should().StartWith($"Property '{nameof(TestClass.Bool)}' of type '{nameof(Boolean)}' is not compatible with");
    }

    public static IEnumerable<object[]> BoolTestCases => new[]
    {
        new object[] { true, new[] { "true" }, SearchOperator.Equals, true },
        new object[] { false, new[]{ "false" }, SearchOperator.Equals, true },
        new object[] { true, new[] { "false" }, SearchOperator.Equals, false },
        new object[] { false, new[] { "true" }, SearchOperator.Equals, false },

        new object[] { true, new[] { "true" }, SearchOperator.NotEquals, false },
        new object[] { false, new[] { "false" }, SearchOperator.NotEquals, false },
        new object[] { true, new[] { "false" }, SearchOperator.NotEquals, true },
        new object[] { false, new[] { "true" }, SearchOperator.NotEquals, true },

        new object[] { true, new[] { "true" }, SearchOperator.InRange, true },
        new object[] { false, new[] { "false" }, SearchOperator.InRange, true },
        new object[] { true, new[] { "false" }, SearchOperator.InRange, false },
        new object[] { false, new[] { "true" }, SearchOperator.InRange, false },
        new object[] { true, Array.Empty<string?>(), SearchOperator.InRange, false },
    };

    public static IEnumerable<object?[]> NullableBoolTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[]{ string.Empty }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "true" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { "false" }, SearchOperator.Equals, false },
        new object?[] { true, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { false, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "true" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { "false" }, SearchOperator.NotEquals, true },
        new object?[] { true, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { false, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { true, null, SearchOperator.Exists, true },
        new object?[] { false, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { true, null, SearchOperator.NotExists, false },
        new object?[] { false, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.InRange, false },
        new object?[] { null, new[] { "true" }, SearchOperator.InRange, false },
        new object?[] { null, new string?[] { null }, SearchOperator.InRange, true }
    };

    private class TestClass
    {
        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }
    }
}