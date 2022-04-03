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
            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Equals, false };

            yield return new object[] { default(char), default(char).ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.NotEquals, true };
            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.NotEquals, true };

            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { default(char), default(char).ToString(), SearchOperator.Greater, false };
            yield return new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Greater, false };

            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object[] { default(char), default(char).ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true };

            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Less, true };
            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Less, false };
            yield return new object[] { default(char), default(char).ToString(), SearchOperator.Less, false };
            yield return new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.Less, false };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Less, false };

            yield return new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.LessOrEqual, false };
            yield return new object[] { default(char), default(char).ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.LessOrEqual, true };
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

            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { null, char.MaxValue.ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { default(char), null, SearchOperator.NotEquals, true };
            yield return new object?[] { char.MaxValue, null, SearchOperator.NotEquals, true };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, char.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(char), null, SearchOperator.Greater, false };
            yield return new object?[] { char.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { null, null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, char.MinValue.ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { default(char), null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { char.MaxValue, null, SearchOperator.GreaterOrEqual, false };

            yield return new object?[] { null, null, SearchOperator.Less, false };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.Less, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Less, false };
            yield return new object?[] { null, char.MaxValue.ToString(), SearchOperator.Less, false };
            yield return new object?[] { default(char), null, SearchOperator.Less, false };
            yield return new object?[] { char.MinValue, null, SearchOperator.Less, false };

            yield return new object?[] { null, null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, default(char).ToString(), SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, char.MaxValue.ToString(), SearchOperator.LessOrEqual, false };
            yield return new object?[] { default(char), null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { char.MinValue, null, SearchOperator.LessOrEqual, false };

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