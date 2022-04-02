using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class CharTests
{
    public static IEnumerable<object[]> CharTestCases
    {
        get
        {
            yield return new object[] { default(char), default(char).ToString(), SearchOperator.Equals, true };
            yield return new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Equals, false };

            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { default(char), default(char).ToString(), SearchOperator.Greater, false };
            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Greater, false };
        }
    }

    public static IEnumerable<object?[]> NullableCharTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, char.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(char), null, SearchOperator.Equals, false };
            yield return new object?[] { char.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, char.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(char), null, SearchOperator.Greater, false };
            yield return new object?[] { char.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { char.MaxValue, null, SearchOperator.Exists, true };
            yield return new object?[] { default(byte), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { char.MaxValue, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(char), null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(CharTestCases))]
    public void ShouldHandleChar(
        char propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Char = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Char),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(CharTestCases))]
    [MemberData(nameof(NullableCharTestCases))]
    public void ShouldHandleNullableChar(
        char? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableChar = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableChar),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}