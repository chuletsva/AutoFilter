using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class ShortTests
{
    public static IEnumerable<object[]> ShortTestCases
    {
        get
        {
            yield return new object[] { default(short), default(short).ToString(), SearchOperator.Equals, true };
            yield return new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.Equals, false };

            yield return new object[] { short.MaxValue, short.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { default(short), default(short).ToString(), SearchOperator.Greater, false };
            yield return new object[] { short.MinValue, short.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { short.MaxValue, short.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { short.MinValue, short.MaxValue.ToString(), SearchOperator.Greater, false };
        }
    }

    public static IEnumerable<object?[]> NullableShortTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(short).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, short.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(short), null, SearchOperator.Equals, false };
            yield return new object?[] { short.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(short).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, short.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(short), null, SearchOperator.Greater, false };
            yield return new object?[] { short.MaxValue, null, SearchOperator.Greater, false };
        }
    }

    [Theory]
    [MemberData(nameof(ShortTestCases))]
    public void ShouldHandleShort(
        short propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Short = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Short),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ShortTestCases))]
    [MemberData(nameof(NullableShortTestCases))]
    public void ShouldHandleNullableShort(
        short? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableShort = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableShort),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}
