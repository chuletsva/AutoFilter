using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class DecimalTests
{
    class TestClass
    {
        public decimal Decimal { get; init; }
        public decimal? NullableDecimal { get; init; }
    }

    public static IEnumerable<object[]> DecimalTestCases => new[]
    {
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { 1M, "1.0", SearchOperator.Equals, true },
        new object[] { 1M, "1,0", SearchOperator.Equals, true },

        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { 1M, "1.0", SearchOperator.NotEquals, false },
        new object[] { 1M, "1,0", SearchOperator.NotEquals, false },

        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { 1.1M, "1.0", SearchOperator.Greater, true },
        new object[] { 1.1M, "1,0", SearchOperator.Greater, true },

        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1M, "1.0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1M, "1,0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1M, "1.0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1M, "1,0", SearchOperator.GreaterOrEqual, true },

        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { 1.0M, "1.1", SearchOperator.Less, true },
        new object[] { 1.0M, "1,1", SearchOperator.Less, true },

        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { 1.0M, "1.1", SearchOperator.LessOrEqual, true },
        new object[] { 1.0M, "1,1", SearchOperator.LessOrEqual, true },
        new object[] { 1M, "1.0", SearchOperator.LessOrEqual, true },
        new object[] { 1M, "1,0", SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableDecimalTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { null, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { default(decimal), null, SearchOperator.Equals, false },
        new object?[] { decimal.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { null, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { default(decimal), null, SearchOperator.NotEquals, true },
        new object?[] { decimal.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { default(decimal), null, SearchOperator.Greater, false },
        new object?[] { decimal.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(decimal), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { decimal.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { default(decimal), null, SearchOperator.Less, false },
        new object?[] { decimal.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { default(decimal), null, SearchOperator.LessOrEqual, false },
        new object?[] { decimal.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { decimal.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(decimal), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { decimal.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(decimal), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    public void ShouldHandleDecimal(
        decimal propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Decimal = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Decimal),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    [MemberData(nameof(NullableDecimalTestCases))]
    public void ShouldHandleNullableDecimal(
        decimal? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableDecimal = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableDecimal),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }
}