using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class BoolTests
{
    public static IEnumerable<object[]> BoolTestCases
    {
        get
        {
            yield return new object[] { true, "true", SearchOperator.Equals, true };
            yield return new object[] { false, "false", SearchOperator.Equals, true };
            yield return new object[] { true, "false", SearchOperator.Equals, false };
            yield return new object[] { false, "true", SearchOperator.Equals, false };
        }
    }

    public static IEnumerable<object?[]> NullableBoolTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, "true", SearchOperator.Equals, false };
            yield return new object?[] { null, "false", SearchOperator.Equals, false };
            yield return new object?[] { true, null, SearchOperator.Equals, false };
            yield return new object?[] { false, null, SearchOperator.Equals, false };

            yield return new object?[] { true, null, SearchOperator.Exists, true };
            yield return new object?[] { false, null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { true, null, SearchOperator.NotExists, false };
            yield return new object?[] { false, null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(BoolTestCases))]
    public void ShouldHandleBool(
        bool propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Bool = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Bool),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(BoolTestCases))]
    [MemberData(nameof(NullableBoolTestCases))]
    public void ShouldHandleNullableBool(
        bool? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableBool = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableBool),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}