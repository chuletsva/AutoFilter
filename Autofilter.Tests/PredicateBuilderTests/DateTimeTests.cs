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
    public static IEnumerable<object?[]> DateTimeTestCases
    {
        get
        {
            yield return new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false }; 

            yield return new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };

            yield return new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };

            yield return new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true };

            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };

            yield return new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
            yield return new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true };
        }
    }

    public static IEnumerable<object?[]> NullableDateTimeTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object?[] { null, DateTime.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false };
            yield return new object?[] { default(DateTime), null, SearchOperator.Equals, false };
            yield return new object?[] { DateTime.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object?[] { null, DateTime.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true };
            yield return new object?[] { default(DateTime), null, SearchOperator.NotEquals, true };
            yield return new object?[] { DateTime.MaxValue, null, SearchOperator.NotEquals, true };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, DateTime.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false };
            yield return new object?[] { default(DateTime), null, SearchOperator.Greater, false };
            yield return new object?[] { DateTime.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { null, null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, DateTime.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { default(DateTime), null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { DateTime.MaxValue, null, SearchOperator.GreaterOrEqual, false };

            yield return new object?[] { null, null, SearchOperator.Less, false };
            yield return new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Less, false };
            yield return new object?[] { null, DateTime.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false };
            yield return new object?[] { default(DateTime), null, SearchOperator.Less, false };
            yield return new object?[] { DateTime.MinValue, null, SearchOperator.Less, false };

            yield return new object?[] { null, null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, DateTime.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false };
            yield return new object?[] { default(DateTime), null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { DateTime.MinValue, null, SearchOperator.LessOrEqual, false };

            yield return new object?[] { DateTime.Now, null, SearchOperator.Exists, true };
            yield return new object?[] { default(DateTime), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { DateTime.Now, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(DateTime), null, SearchOperator.NotExists, false };
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