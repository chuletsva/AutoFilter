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
            yield return new object[] { default(TestEnum), default(TestEnum).ToString(), SearchOperator.Equals, true };
            yield return new object[] { TestEnum.One, TestEnum.Two.ToString(), SearchOperator.Equals, false };

            yield return new object[] { default(TestEnum), default(TestEnum).ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { TestEnum.One, TestEnum.Two.ToString(), SearchOperator.NotEquals, true };
        }
    }

    public static IEnumerable<object?[]> NullableEnumTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(TestEnum).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, TestEnum.One.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(TestEnum), null, SearchOperator.Equals, false };
            yield return new object?[] { TestEnum.One, null, SearchOperator.Equals, false };   
            
            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(TestEnum).ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { null, TestEnum.One.ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { default(TestEnum), null, SearchOperator.NotEquals, true };
            yield return new object?[] { TestEnum.One, null, SearchOperator.NotEquals, true };

            yield return new object?[] { TestEnum.One, null, SearchOperator.Exists, true };
            yield return new object?[] { default(TestEnum), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { TestEnum.One, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(TestEnum), null, SearchOperator.NotExists, false };
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