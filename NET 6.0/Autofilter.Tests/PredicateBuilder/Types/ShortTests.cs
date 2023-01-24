using System.Linq.Expressions;
using Autofilter.Models;
using FluentAssertions;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class ShortTests
{
    class TestClass
    {
        public short Short { get; init; }
        public short? NullableShort { get; init; }
    }

    public static IEnumerable<object[]> ShortTestCases => new[]
    {
        new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.Equals, false },
        new object[] { default(short), default(short).ToString(), SearchOperator.Equals, true },
        new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.Equals, true },

        new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { default(short), default(short).ToString(), SearchOperator.NotEquals, false },
        new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.NotEquals, false },

        new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(short), default(short).ToString(), SearchOperator.Greater, false },
        new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(short), default(short).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(short), default(short).ToString(), SearchOperator.Less, false },
        new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(short), default(short).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableShortTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(short).ToString(), SearchOperator.Equals, false },
        new object?[] { null, short.MaxValue.ToString(), SearchOperator.Equals, false },
        new object?[] { default(short), null, SearchOperator.Equals, false },
        new object?[] { short.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(short).ToString(), SearchOperator.NotEquals, true },
        new object?[] { null, short.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(short), null, SearchOperator.NotEquals, true },
        new object?[] { short.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(short).ToString(), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, short.MinValue.ToString(), SearchOperator.Greater, false },
        new object?[] { default(short), null, SearchOperator.Greater, false },
        new object?[] { short.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(short).ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, short.MinValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(short), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { short.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(short).ToString(), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, short.MaxValue.ToString(), SearchOperator.Less, false },
        new object?[] { default(short), null, SearchOperator.Less, false },
        new object?[] { short.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(short).ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, short.MaxValue.ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { default(short), null, SearchOperator.LessOrEqual, false },
        new object?[] { short.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { short.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(short), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { short.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(short), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(ShortTestCases))]
    public void ShouldHandleShort(short propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Short = propValue };

        SearchRule rule = new
        (
            Name: nameof(obj.Short),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ShortTestCases))]
    [MemberData(nameof(NullableShortTestCases))]
    public void ShouldHandleNullableShort(short? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableShort = propValue };

        SearchRule rule = new
        (
            Name: nameof(obj.NullableShort),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }
}
