using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class EnumTests
{
    public enum TestEnum { One, Two }

    class TestClass
    {
        public TestEnum Enum { get; init; }
        public TestEnum? NullableEnum { get; init; }
    }

    public static IEnumerable<object[]> EnumTestCases => new[]
    {
        new object[] { default(TestEnum), default(TestEnum).ToString(), SearchOperator.Equals, true },
        new object[] { TestEnum.One, TestEnum.Two.ToString(), SearchOperator.Equals, false },

        new object[] { default(TestEnum), default(TestEnum).ToString(), SearchOperator.NotEquals, false },
        new object[] { TestEnum.One, TestEnum.Two.ToString(), SearchOperator.NotEquals, true },
    };

    public static IEnumerable<object?[]> NullableEnumTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(TestEnum).ToString(), SearchOperator.Equals, false },
        new object?[] { null, TestEnum.One.ToString(), SearchOperator.Equals, false },
        new object?[] { default(TestEnum), null, SearchOperator.Equals, false },
        new object?[] { TestEnum.One, null, SearchOperator.Equals, false },   

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(TestEnum).ToString(), SearchOperator.NotEquals, true },
        new object?[] { null, TestEnum.One.ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(TestEnum), null, SearchOperator.NotEquals, true },
        new object?[] { TestEnum.One, null, SearchOperator.NotEquals, true },

        new object?[] { TestEnum.One, null, SearchOperator.Exists, true },
        new object?[] { default(TestEnum), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { TestEnum.One, null, SearchOperator.NotExists, false },
        new object?[] { default(TestEnum), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(EnumTestCases))]
    public void ShouldHandleEnum(
        TestEnum propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Enum = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Enum),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(EnumTestCases))]
    [MemberData(nameof(NullableEnumTestCases))]
    public void ShouldHandleNullableEnum(
        TestEnum? propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableEnum = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableEnum),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }
}