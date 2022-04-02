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

            yield return new object[] { byte.MaxValue, byte.MinValue.ToString(), SearchOperator.Greater, true };
            yield return new object[] { default(byte), default(byte).ToString(), SearchOperator.Greater, false };
            yield return new object[] { byte.MinValue, byte.MinValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { byte.MaxValue, byte.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object[] { byte.MinValue, byte.MaxValue.ToString(), SearchOperator.Greater, false };
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
            yield return new object?[] { null, byte.MaxValue.ToString(), SearchOperator.Greater, false };
            yield return new object?[] { default(byte), null, SearchOperator.Greater, false };
            yield return new object?[] { byte.MaxValue, null, SearchOperator.Greater, false };
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