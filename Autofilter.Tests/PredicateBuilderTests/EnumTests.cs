using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class EnumTests
{
    public static IEnumerable<object[]> EnumTestCases
    {
        get
        {
            yield return new object[] { TestEnum.One, "One", SearchOperator.Equals, true };
            yield return new object[] { TestEnum.One, "Two", SearchOperator.Equals, false };
        }
    }

    public static IEnumerable<object?[]> NullableEnumTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, "", SearchOperator.Equals, true };
            yield return new object?[] { null, "One", SearchOperator.Equals, false };
        }
    }

    [Theory]
    [MemberData(nameof(EnumTestCases))]
    public void ShouldHandleEnum(
        TestEnum propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Enum = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Enum),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(EnumTestCases))]
    [MemberData(nameof(NullableEnumTestCases))]
    public void ShouldHandleNullableEnum(
        TestEnum? propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableEnum = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableEnum),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}