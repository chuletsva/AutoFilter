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
            yield return new object[] { 0F, "0", SearchOperator.Equals, true };
            yield return new object[] { 1F, "1", SearchOperator.Equals, true };
            yield return new object[] { -1F, "-1", SearchOperator.Equals, true };
            yield return new object[] { float.MinValue, float.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.MaxValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.NegativeInfinity, float.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.PositiveInfinity, float.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.Epsilon, float.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { float.MinValue, float.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };

            yield return new object[] { 1F, "1.0", SearchOperator.Equals, true };
            yield return new object[] { 1F, "1,0", SearchOperator.Equals, true };
        }
    }

    public static IEnumerable<object?[]> NullableFloatTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, "", SearchOperator.Equals, true };
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