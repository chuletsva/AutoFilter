using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class ByteTests
{
    public static IEnumerable<object[]> ByteTestCases
    {
        get
        {
            yield return new object[] { default(byte), default(byte).ToString(), SearchOperator.Equals, true };
            yield return new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Equals, true };
            yield return new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Equals, false };

            yield return new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { default(byte), default(byte).ToString(), SearchOperator.Greater, false };
            yield return new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Greater, false };

            yield return new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object[] { default(byte), default(byte).ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.GreaterOrEqual, true };
            yield return new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true };

            yield return new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Less, true };
            yield return new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Less, false };
            yield return new object[] { default(byte), default(byte).ToString(), SearchOperator.Less, false };
            yield return new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Less, false };
            yield return new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Less, false };

            yield return new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.LessOrEqual, false };
            yield return new object[] { default(byte), default(byte).ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.LessOrEqual, true };
            yield return new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.LessOrEqual, true };
        }
    }

    public static IEnumerable<object?[]> NullableByteTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(byte).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, byte.MaxValue.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(byte), null, SearchOperator.Equals, false };
            yield return new object?[] { byte.MaxValue, null, SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Greater, false };
            yield return new object?[] { null, default(byte).ToString(), SearchOperator.Greater, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Greater, false };
            yield return new object?[] { null, byte.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(byte), null, SearchOperator.Greater, false };
            yield return new object?[] { byte.MaxValue, null, SearchOperator.Greater, false };

            yield return new object?[] { null, null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, default(byte).ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { null, byte.MinValue.ToString(), SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { default(byte), null, SearchOperator.GreaterOrEqual, false };
            yield return new object?[] { byte.MaxValue, null, SearchOperator.GreaterOrEqual, false };

            yield return new object?[] { null, null, SearchOperator.Less, false };
            yield return new object?[] { null, default(byte).ToString(), SearchOperator.Less, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Less, false };
            yield return new object?[] { null, byte.MaxValue.ToString(), SearchOperator.Less, false };
            yield return new object?[] { default(byte), null, SearchOperator.Less, false };
            yield return new object?[] { byte.MinValue, null, SearchOperator.Less, false };

            yield return new object?[] { null, null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, default(byte).ToString(), SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false };
            yield return new object?[] { null, byte.MaxValue.ToString(), SearchOperator.LessOrEqual, false };
            yield return new object?[] { default(byte), null, SearchOperator.LessOrEqual, false };
            yield return new object?[] { byte.MinValue, null, SearchOperator.LessOrEqual, false };

            yield return new object?[] { byte.MaxValue, null, SearchOperator.Exists, true };
            yield return new object?[] { default(byte), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { byte.MaxValue, null, SearchOperator.NotExists, false };
            yield return new object?[] { default(byte), null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(ByteTestCases))]
    public void ShouldHandleByte(
        byte propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Byte = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Byte),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ByteTestCases))]
    [MemberData(nameof(NullableByteTestCases))]
    public void ShouldHandleNullableByte(
        byte? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableByte = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableByte),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}