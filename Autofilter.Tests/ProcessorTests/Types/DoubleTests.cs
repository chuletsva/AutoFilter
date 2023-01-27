using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests.Types;

public class DoubleTests
{
    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    public void ShouldHandleDouble(double objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Double = objValue };

        Condition condition = new(nameof(obj.Double), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    [MemberData(nameof(NullableDoubleTestCases))]
    public void ShouldHandleNullableDouble(double? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDouble = objValue };

        Condition condition = new(nameof(obj.NullableDouble), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

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

    private class TestClass
    {
        public double Double { get; init; }
        public double? NullableDouble { get; init; }
    }
}