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

public class DateTimeTests
{
    public static IEnumerable<object[]> DateTimeTestCases
    {
        get
        {
            DateTime now = DateTime.Now.Date;
            yield return new object[] { now, now.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
        }
    }

    public static IEnumerable<object?[]> NullableDateTimeTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, "", SearchOperator.Equals, true };
            yield return new object?[] { null, " ", SearchOperator.Equals, false };
        }
    }

    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    public void ShouldHandleDateTime(
        DateTime propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { DateTime = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.DateTime),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    [MemberData(nameof(NullableDateTimeTestCases))]
    public void ShouldHandleNullableDateTime(
        DateTime? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableDateTime = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableDateTime),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}