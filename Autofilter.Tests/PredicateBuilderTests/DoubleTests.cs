﻿using System;
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
            yield return new object[] { 0D, "0", SearchOperator.Equals, true };
            yield return new object[] { 1D, "1", SearchOperator.Equals, true };
            yield return new object[] { -1D, "-1", SearchOperator.Equals, true };
            yield return new object[] { double.MinValue, double.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.MaxValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.NegativeInfinity, double.NegativeInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.PositiveInfinity, double.PositiveInfinity.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.Epsilon, double.Epsilon.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { double.MinValue, double.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };

            yield return new object[] { 1D, "1.0", SearchOperator.Equals, true };
            yield return new object[] { 1D, "1,0", SearchOperator.Equals, true };
        }
    }

    public static IEnumerable<object?[]> NullableDoubleTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, "", SearchOperator.Equals, true };
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