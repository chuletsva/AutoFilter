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

public class DecimalTests
{
    public static IEnumerable<object[]> DecimalTestCases
    {
        get
        {
            yield return new object[] { decimal.Zero, decimal.Zero.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { decimal.One, decimal.One.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { decimal.MinusOne, decimal.MinusOne.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { decimal.MinValue, decimal.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { decimal.MaxValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { decimal.MinValue, decimal.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };

            yield return new object[] { 1M, "1.0", SearchOperator.Equals, true };
            yield return new object[] { 1M, "1,0", SearchOperator.Equals, true };
        }
    }

    public static IEnumerable<object?[]> NullableDecimalTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, "", SearchOperator.Equals, true };
        }
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    public void ShouldHandleDecimal(
        decimal propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Decimal = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Decimal),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    [MemberData(nameof(NullableDecimalTestCases))]
    public void ShouldHandleNullableDecimal(
        decimal? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableDecimal = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableDecimal),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}