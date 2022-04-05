using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class BoolTests
{
    class TestClass
    {
        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }
    }

    public static IEnumerable<object[]> BoolTestCases
    {
        get
        {
            yield return new object[] { true, "true", SearchOperator.Equals, true };
            yield return new object[] { false, "false", SearchOperator.Equals, true };
            yield return new object[] { true, "false", SearchOperator.Equals, false };
            yield return new object[] { false, "true", SearchOperator.Equals, false };

            yield return new object[] { true, "true", SearchOperator.NotEquals, false };
            yield return new object[] { false, "false", SearchOperator.NotEquals, false };
            yield return new object[] { true, "false", SearchOperator.NotEquals, true };
            yield return new object[] { false, "true", SearchOperator.NotEquals, true };
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

            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { null, "true", SearchOperator.NotEquals, true };
            yield return new object?[] { null, "false", SearchOperator.NotEquals, true };
            yield return new object?[] { true, null, SearchOperator.NotEquals, true };
            yield return new object?[] { false, null, SearchOperator.NotEquals, true };

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
        TestClass obj = new() { Bool = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Bool),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
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

        Expression<Func<TestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
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

        Action act = () => PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

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

        Action act = () => PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        act.Should().Throw<Exception>()
            .WithMessage($"Cannot convert value '{value}' to type '{nameof(Boolean)}'")
            .WithInnerException<Exception>();
    }
}