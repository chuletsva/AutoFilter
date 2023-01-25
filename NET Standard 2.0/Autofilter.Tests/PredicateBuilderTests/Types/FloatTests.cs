using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class FloatTests
{
    public static IEnumerable<object[]> FloatTestCases => new[]
    {
        new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { 1F, "1.0", SearchOperator.Equals, true },

        new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { 1F, "1.0", SearchOperator.NotEquals, false },

        new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { float.PositiveInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { 1.1F, "1.0", SearchOperator.Greater, true },

        new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { float.PositiveInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1F, "1.0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1F, "1.0", SearchOperator.GreaterOrEqual, true },

        new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { float.NegativeInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { 1.0F, "1.1", SearchOperator.Less, true },

        new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { float.NegativeInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { 1.0F, "1.1", SearchOperator.LessOrEqual, true },
        new object[] { 1F, "1.0", SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableFloatTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { default(float), null, SearchOperator.Equals, false },
        new object?[] { float.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { default(float), null, SearchOperator.NotEquals, true },
        new object?[] { float.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { default(float), null, SearchOperator.Greater, false },
        new object?[] { float.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(float), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { float.MinValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { default(float), null, SearchOperator.Less, false },
        new object?[] { float.MaxValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { default(float), null, SearchOperator.LessOrEqual, false },
        new object?[] { float.MaxValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { float.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(float), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { float.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(float), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(FloatTestCases))]
    public void ShouldHandleFloat(float objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Float = objValue };

        SearchRule rule = new(nameof(obj.Float), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(FloatTestCases))]
    [MemberData(nameof(NullableFloatTestCases))]
    public void ShouldHandleNullableFloat(float? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableFloat = objValue };

        SearchRule rule = new(nameof(obj.NullableFloat), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    private class TestClass
    {
        public float Float { get; init; }
        public float? NullableFloat { get; init; }
    }
}