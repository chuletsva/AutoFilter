using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests.Types;

public class IntTests
{
    [Theory]
    [MemberData(nameof(IntTestCases))]
    public void ShouldHandleInt(int objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Int = objValue };

        Condition condition = new(nameof(obj.Int), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(IntTestCases))]
    [MemberData(nameof(NullableIntTestCases))]
    public void ShouldHandleNullableInt(int? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableInt = objValue };

        Condition condition = new(nameof(obj.NullableInt), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> IntTestCases => new[]
    {
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Equals, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Equals, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Equals, true },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { default(int), default(int).ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.NotEquals, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Greater, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Less, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableIntTestCases => new[]
    {
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Equals, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Equals, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Equals, true },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { default(int), default(int).ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.NotEquals, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Greater, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Less, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    private class TestClass
    {
        public int Int { get; init; }
        public int? NullableInt { get; init; }
    }
}