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

public class FloatTests
{
    public static IEnumerable<object[]> FloatTestCases
    {
        get
        {
            yield return new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object[] { 1F, "1.0", SearchOperator.Equals, true };
            yield return new object[] { 1F, "1,0", SearchOperator.Equals, true };

            yield return new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object[] { 1F, "1.0", SearchOperator.NotEquals, false };
            yield return new object[] { 1F, "1,0", SearchOperator.NotEquals, false };

            yield return new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true };
            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { float.PositiveInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true };
            yield return new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { 1.1F, "1.0", SearchOperator.Greater, true };
            yield return new object[] { 1.1F, "1,0", SearchOperator.Greater, true };

            yield return new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object[] { float.PositiveInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1.1F, "1.0", SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1.1F, "1,0", SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1F, "1.0", SearchOperator.GreaterOrEqual, true };
            yield return new object[] { 1F, "1,0", SearchOperator.GreaterOrEqual, true };

            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true };
            yield return new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { float.NegativeInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true };
            yield return new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { 1.0F, "1.1", SearchOperator.Less, true };
            yield return new object[] { 1.0F, "1,1", SearchOperator.Less, true };

            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { float.MaxValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object[] { float.NegativeInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { default(float), default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { 1.0F, "1.1", SearchOperator.LessOrEqual, true };
            yield return new object[] { 1.0F, "1,1", SearchOperator.LessOrEqual, true };
            yield return new object[] { 1F, "1.0", SearchOperator.LessOrEqual, true };
            yield return new object[] { 1F, "1,0", SearchOperator.LessOrEqual, true };
        }
    }

    public static IEnumerable<object?[]> NullableFloatTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object?[] { default(float), null, SearchOperator.Equals, false };
            yield return new object?[] { float.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object?[] { default(float), null, SearchOperator.NotEquals, true };
            yield return new object?[] { float.MaxValue, null, SearchOperator.NotEquals, true };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object?[] { default(float), null, SearchOperator.Greater, false };
            yield return new object?[] { float.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { null, null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { default(float), null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { float.MinValue, null, SearchOperator.GreaterOrEqual, false };

            yield return new object?[] { null, null, SearchOperator.Less, false };
            yield return new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Less, false };
            yield return new object?[] { null, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object?[] { default(float), null, SearchOperator.Less, false };
            yield return new object?[] { float.MaxValue, null, SearchOperator.Less, false };

            yield return new object?[] { null, null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, default(float).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object?[] { default(float), null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { float.MaxValue, null, SearchOperator.LessOrEqual, false };

            yield return new object?[] { float.MaxValue, null, SearchOperator.Exists, true };
            yield return new object?[] { default(float), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { float.MaxValue, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(float), null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(FloatTestCases))]
    public void ShouldHandleFloat(
        float propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Float = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Float),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(FloatTestCases))]
    [MemberData(nameof(NullableFloatTestCases))]
    public void ShouldHandleNullableFloat(
        float? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableFloat = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableFloat),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}