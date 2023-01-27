using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests.Types;

public class BoolTests
{
    [Theory]
    [MemberData(nameof(BoolTestCases))]
    public void ShouldHandleBool(bool objValue, string searchValue, SearchOperator searchOperator, bool result)
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
    public void ShouldHandleNullableBool(bool? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableBool = objValue };

        Condition condition = new(nameof(obj.NullableBool), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [InlineData(null)]
    public void ShouldThrow_WhenNotComparableValue(string? searchValue)
    {
        Condition condition = new(nameof(TestClass.Bool), searchValue, SearchOperator.Equals);

        FluentActions.Invoking(() => FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition }))
            .Should().Throw<Exception>().Which.Message
            .Should().StartWith($"Property '{nameof(TestClass.Bool)}' of type '{nameof(Boolean)}' is not comparable with");
    }

    [Theory]
    [InlineData("")]
    public void ShouldThrow_WhenNotConvertableValue(string searchValue)
    {
        Condition condition = new(nameof(TestClass.Bool), searchValue, SearchOperator.Equals);

        FluentActions.Invoking(() => FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition }))
            .Should().Throw<Exception>().Which.Message.Should().StartWith("Cannot convert");
    }

    public static IEnumerable<object[]> BoolTestCases => new[]
    {
        new object[] { true, "true", SearchOperator.Equals, true },
        new object[] { false, "false", SearchOperator.Equals, true },
        new object[] { true, "false", SearchOperator.Equals, false },
        new object[] { false, "true", SearchOperator.Equals, false },

        new object[] { true, "true", SearchOperator.NotEquals, false },
        new object[] { false, "false", SearchOperator.NotEquals, false },
        new object[] { true, "false", SearchOperator.NotEquals, true },
        new object[] { false, "true", SearchOperator.NotEquals, true },
    };

    public static IEnumerable<object?[]> NullableBoolTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, "true", SearchOperator.Equals, false },
        new object?[] { null, "false", SearchOperator.Equals, false },
        new object?[] { true, null, SearchOperator.Equals, false },
        new object?[] { false, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, "true", SearchOperator.NotEquals, true },
        new object?[] { null, "false", SearchOperator.NotEquals, true },
        new object?[] { true, null, SearchOperator.NotEquals, true },
        new object?[] { false, null, SearchOperator.NotEquals, true },

        new object?[] { true, null, SearchOperator.Exists, true },
        new object?[] { false, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { true, null, SearchOperator.NotExists, false },
        new object?[] { false, null, SearchOperator.NotExists, false },
    };

    private class TestClass
    {
        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }
    }
}