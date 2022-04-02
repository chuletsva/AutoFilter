using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class LongTests
{
    public static IEnumerable<object[]> LongTestCases
    {
        get
        {
            yield return new object[] { default(long), default(long).ToString(), SearchOperator.Equals, true };
            yield return new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.Equals, false };

            yield return new object[] { long.MaxValue, long.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { default(long), default(long).ToString(), SearchOperator.Greater, false };
            yield return new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.Greater, false };

        }
    }

    public static IEnumerable<object?[]> NullableLongTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(long).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, long.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(long), null, SearchOperator.Equals, false };
            yield return new object?[] { long.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(long).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, long.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(long), null, SearchOperator.Greater, false };
            yield return new object?[] { long.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { long.MaxValue, null, SearchOperator.Exists, true };
            yield return new object?[] { default(long), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { long.MaxValue, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(long), null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(LongTestCases))]
    public void ShouldHandleLong(
        long propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Long = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Long),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(LongTestCases))]
    [MemberData(nameof(NullableLongTestCases))]
    public void ShouldHandleNullableLong(
        long? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableLong = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableLong),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}