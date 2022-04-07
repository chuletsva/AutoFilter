using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class BoolTests
{
    class TestClass
    {
        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }
    }

    public static IEnumerable<object[]> BoolTestCases => new[]
    {
        new object[] { true, "true", SearchOperator.Equals, true },
        new object[] { false, "false", SearchOperator.Equals, true },
        new object[] { true, "false", SearchOperator.Equals, false },
        new object[] { false, "true", SearchOperator.Equals, false },

        new object[] { true, "true", SearchOperator.NotEquals, false },
        new object[] { false, "false", SearchOperator.NotEquals, false },
        new object[] { true, "false", SearchOperator.NotEquals, true },
        new object[] { false, "true", SearchOperator.NotEquals, true },
    };

    public static IEnumerable<object?[]> NullableBoolTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, "true", SearchOperator.Equals, false },
        new object?[] { null, "false", SearchOperator.Equals, false },
        new object?[] { true, null, SearchOperator.Equals, false },
        new object?[] { false, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, "true", SearchOperator.NotEquals, true },
        new object?[] { null, "false", SearchOperator.NotEquals, true },
        new object?[] { true, null, SearchOperator.NotEquals, true },
        new object?[] { false, null, SearchOperator.NotEquals, true },

        new object?[] { true, null, SearchOperator.Exists, true },
        new object?[] { false, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { true, null, SearchOperator.NotExists, false },
        new object?[] { false, null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(BoolTestCases))]
    public void ShouldHandleBool(
        bool propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Bool = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Bool),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(BoolTestCases))]
    [MemberData(nameof(NullableBoolTestCases))]
    public void ShouldHandleNullableBool(
        bool? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableBool = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableBool),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [InlineData(null, "null")]
    public void ShouldThrow_WhenPassedNotComparableValue(string? value, string valueAlias)
    {
        SearchRule rule = new
        (
            PropertyName: nameof(TestClass.Bool),
            Value: value,
            SearchOperator: SearchOperator.Equals
        );

        Action act = () => BuildPredicate<TestClass>(new[] { rule });

        string expectedMessage = $"Property '{nameof(TestClass.Bool)}' with type '{nameof(Boolean)}' is not comparable with {valueAlias}";

        act.Should().Throw<Exception>().WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("")]
    public void ShouldThrow_WhenPassedNotConvertableValue(string value)
    {
        SearchRule rule = new
        (
            PropertyName: nameof(TestClass.Bool),
            Value: value,
            SearchOperator: SearchOperator.Equals
        );

        Action act = () => BuildPredicate<TestClass>(new[] { rule });

        act.Should().Throw<Exception>()
            .WithMessage($"Cannot convert value '{value}' to type '{nameof(Boolean)}'")
            .WithInnerException<Exception>();
    }
}