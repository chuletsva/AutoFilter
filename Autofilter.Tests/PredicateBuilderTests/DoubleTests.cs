using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class DoubleTests
{
    public static IEnumerable<object[]> DoubleTestCases
    {
        get
        {
            yield return new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object[] { 1D, "1.0", SearchOperator.Equals, true };
            yield return new object[] { 1D, "1,0", SearchOperator.Equals, true };

            yield return new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object[] { 1D, "1.0", SearchOperator.NotEquals, false };
            yield return new object[] { 1D, "1,0", SearchOperator.NotEquals, false };

            yield return new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true };
            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true };
            yield return new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { 1.1D, "1.0", SearchOperator.Greater, true };
            yield return new object[] { 1.1D, "1,0", SearchOperator.Greater, true };

            yield return new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object[] { double.PositiveInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1.1D, "1.0", SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1.1D, "1,0", SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1D, "1.0", SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1D, "1,0", SearchOperator.GreaterOrEqual, true };

            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true };
            yield return new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true };
            yield return new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { 1.0D, "1.1", SearchOperator.Less, true };
            yield return new object[] { 1.0D, "1,1", SearchOperator.Less, true };

            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { double.MaxValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object[] { double.NegativeInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { default(double), default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { 1.0D, "1.1", SearchOperator.LessOrEqual, true };
            yield return new object[] { 1.0D, "1,1", SearchOperator.LessOrEqual, true };
            yield return new object[] { 1D, "1.0", SearchOperator.LessOrEqual, true };
            yield return new object[] { 1D, "1,0", SearchOperator.LessOrEqual, true };
        }
    }

    public static IEnumerable<object?[]> NullableDoubleTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object?[] { null, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object?[] { default(double), null, SearchOperator.Equals, false };
            yield return new object?[] { double.MaxValue, null, SearchOperator.Equals, false };            
            
            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object?[] { null, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object?[] { default(double), null, SearchOperator.NotEquals, true };
            yield return new object?[] { double.MaxValue, null, SearchOperator.NotEquals, true };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object?[] { default(double), null, SearchOperator.Greater, false };
            yield return new object?[] { double.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { null, null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { default(double), null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { double.MinValue, null, SearchOperator.GreaterOrEqual, false };

            yield return new object?[] { null, null, SearchOperator.Less, false };
            yield return new object?[] { null, default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Less, false };
            yield return new object?[] { null, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object?[] { default(double), null, SearchOperator.Less, false };
            yield return new object?[] { double.MaxValue, null, SearchOperator.Less, false };

            yield return new object?[] { null, null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, default(double).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object?[] { default(double), null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { double.MaxValue, null, SearchOperator.LessOrEqual, false };

            yield return new object?[] { double.MaxValue, null, SearchOperator.Exists, true };
            yield return new object?[] { default(double), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { double.MaxValue, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(double), null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    public void ShouldHandleDouble(
        double propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Double = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Double),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    [MemberData(nameof(NullableDoubleTestCases))]
    public void ShouldHandleNullableDouble(
        double? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableDouble = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableDouble),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}