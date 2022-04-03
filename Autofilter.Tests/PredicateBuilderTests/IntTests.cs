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
            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Equals, false };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.Equals, true };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Equals, true };    
            
            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.NotEquals, true };
            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.NotEquals, true };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.NotEquals, false };

            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.Greater, false };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Greater, false };

            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true };

            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Less, true };
            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Less, false };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.Less, false };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Less, false };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Less, false };

            yield return new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, false };
            yield return new object[] { default(int), default(int).ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true };
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
            
            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { null, int.MaxValue.ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { default(int), null, SearchOperator.NotEquals, true };
            yield return new object?[] { int.MaxValue, null, SearchOperator.NotEquals, true };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, int.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(int), null, SearchOperator.Greater, false };
            yield return new object?[] { int.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { null, null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { default(int), null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { int.MaxValue, null, SearchOperator.GreaterOrEqual, false };

            yield return new object?[] { null, null, SearchOperator.Less, false };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.Less, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Less, false };
            yield return new object?[] { null, int.MaxValue.ToString(), SearchOperator.Less, false };
            yield return new object?[] { default(int), null, SearchOperator.Less, false };
            yield return new object?[] { int.MinValue, null, SearchOperator.Less, false };

            yield return new object?[] { null, null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, default(int).ToString(), SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, int.MaxValue.ToString(), SearchOperator.LessOrEqual, false };
            yield return new object?[] { default(int), null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { int.MinValue, null, SearchOperator.LessOrEqual, false };

            yield return new object?[] { int.MaxValue, null, SearchOperator.Exists, true };
            yield return new object?[] { default(int), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { int.MaxValue, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(int), null, SearchOperator.NotExists, false };
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