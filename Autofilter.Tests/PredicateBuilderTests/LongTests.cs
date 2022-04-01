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
            yield return new object[] { 0L, "0", SearchOperator.Equals, true };
            yield return new object[] { long.MinValue, long.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { long.MaxValue, long.MaxValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { long.MinValue, long.MaxValue.ToString(), SearchOperator.Equals, false };
        }
    }

    public static IEnumerable<object?[]> NullableLongTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, "", SearchOperator.Equals, true };
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