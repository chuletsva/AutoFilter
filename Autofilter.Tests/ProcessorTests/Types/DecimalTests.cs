using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests.Types;

public class DecimalTests
{
    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    public void ShouldHandleDecimal(decimal objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Decimal = objValue };

        Condition condition = new(nameof(obj.Decimal), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    [MemberData(nameof(NullableDecimalTestCases))]
    public void ShouldHandleNullableDecimal(decimal? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDecimal = objValue };

        Condition condition = new(nameof(obj.NullableDecimal), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
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

        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { 1M, "1.0", SearchOperator.NotEquals, false },

        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { 1.1M, "1.0", SearchOperator.Greater, true },

        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1M, "1.0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1M, "1.0", SearchOperator.GreaterOrEqual, true },

        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { 1.0M, "1.1", SearchOperator.Less, true },

        new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MaxValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { default(decimal), default(decimal).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { 1.0M, "1.1", SearchOperator.LessOrEqual, true },
        new object[] { 1M, "1.0", SearchOperator.LessOrEqual, true },
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

    private class TestClass
    {
        public decimal Decimal { get; init; }
        public decimal? NullableDecimal { get; init; }
    }
}