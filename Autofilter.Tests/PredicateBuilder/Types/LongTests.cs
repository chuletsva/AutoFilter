using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class LongTests
{
    class TestClass
    {
        public long Long { get; init; }
        public long? NullableLong { get; init; }
    }

    public static IEnumerable<object[]> LongTestCases => new[]
    {
        new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.Equals, false },
        new object[] { default(long), default(long).ToString(), SearchOperator.Equals, true },
        new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.Equals, true },

        new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { default(long), default(long).ToString(), SearchOperator.NotEquals, false },
        new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.NotEquals, false },

        new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(long), default(long).ToString(), SearchOperator.Greater, false },
        new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(long), default(long).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(long), default(long).ToString(), SearchOperator.Less, false },
        new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(long), default(long).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableLongTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(long).ToString(), SearchOperator.Equals, false },
        new object?[] { null, long.MaxValue.ToString(), SearchOperator.Equals, false },
        new object?[] { default(long), null, SearchOperator.Equals, false },
        new object?[] { long.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(long).ToString(), SearchOperator.NotEquals, true },
        new object?[] { null, long.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(long), null, SearchOperator.NotEquals, true },
        new object?[] { long.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(long).ToString(), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, long.MinValue.ToString(), SearchOperator.Greater, false },
        new object?[] { default(long), null, SearchOperator.Greater, false },
        new object?[] { long.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(long).ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, long.MinValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(long), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { long.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(long).ToString(), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, long.MaxValue.ToString(), SearchOperator.Less, false },
        new object?[] { default(long), null, SearchOperator.Less, false },
        new object?[] { long.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(long).ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, long.MaxValue.ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { default(long), null, SearchOperator.LessOrEqual, false },
        new object?[] { long.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { long.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(long), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { long.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(long), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(LongTestCases))]
    public void ShouldHandleLong(
        long propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Long = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Long),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression =
            Helpers.PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(LongTestCases))]
    [MemberData(nameof(NullableLongTestCases))]
    public void ShouldHandleNullableLong(
        long? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableLong = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableLong),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression =
            Helpers.PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}