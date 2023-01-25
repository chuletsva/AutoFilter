using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class DoubleTests
{
    public static IEnumerable<object[]> DoubleTestCases => new[]
    {
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { 1D, "1.0", SearchOperator.Equals, true },

        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { 1D, "1.0", SearchOperator.NotEquals, false },

        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { 1.1D, "1.0", SearchOperator.Greater, true },

        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { double.PositiveInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1D, "1.0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1D, "1.0", SearchOperator.GreaterOrEqual, true },

        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { 1.0D, "1.1", SearchOperator.Less, true },

        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { double.NegativeInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { 1.0D, "1.1", SearchOperator.LessOrEqual, true },
        new object[] { 1D, "1.0", SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableDoubleTestCases => new[]
    {
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object[] { 1D, "1.0", SearchOperator.Equals, true },

        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object[] { 1D, "1.0", SearchOperator.NotEquals, false },

        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { 1.1D, "1.0", SearchOperator.Greater, true },

        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { double.PositiveInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1D, "1.0", SearchOperator.GreaterOrEqual, true },
        new object[] { 1D, "1.0", SearchOperator.GreaterOrEqual, true },

        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { 1.0D, "1.1", SearchOperator.Less, true },

        new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { double.NegativeInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { 1.0D, "1.1", SearchOperator.LessOrEqual, true },
        new object[] { 1D, "1.0", SearchOperator.LessOrEqual, true },
    };

    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    public void ShouldHandleDouble(double objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Double = objValue };

        SearchRule rule = new(nameof(obj.Double), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    [MemberData(nameof(NullableDoubleTestCases))]
    public void ShouldHandleNullableDouble(double? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDouble = objValue };

        SearchRule rule = new(nameof(obj.NullableDouble), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    private class TestClass
    {
        public double Double { get; init; }
        public double? NullableDouble { get; init; }
    }
}