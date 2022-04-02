using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class IntTests
{
    public static IEnumerable<object[]> IntTestCases
    {
        get
        {
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.Equals, true };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Equals, false };

            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.Greater, false };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Greater, false };
        }
    }

    public static IEnumerable<object?[]> NullableIntTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, int.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(int), null, SearchOperator.Equals, false };
            yield return new object?[] { int.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, int.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(int), null, SearchOperator.Greater, false };
            yield return new object?[] { int.MaxValue, null, SearchOperator.Greater, false };
        }
    }

    [Theory]
    [MemberData(nameof(IntTestCases))]
    public void ShouldHandleInt(
        int propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Int = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Int),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(IntTestCases))]
    [MemberData(nameof(NullableIntTestCases))]
    public void ShouldHandleNullableInt(
        int? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableInt = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableInt),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}