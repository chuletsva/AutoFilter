using System.Linq.Expressions;
using Autofilter.Models;
using FluentAssertions;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class CharTests
{
    class TestClass
    {
        public char Char { get; init; }
        public char? NullableChar { get; init; }
    }

    public static IEnumerable<object[]> CharTestCases => new[]
    {
        new object[] { default(char), default(char).ToString(), SearchOperator.Equals, true },
        new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Equals, true },
        new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Equals, false },

        new object[] { default(char), default(char).ToString(), SearchOperator.NotEquals, false },
        new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.NotEquals, true },

        new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(char), default(char).ToString(), SearchOperator.Greater, false },
        new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(char), default(char).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(char), default(char).ToString(), SearchOperator.Less, false },
        new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { char.MinValue, char.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { char.MaxValue, char.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(char), default(char).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { char.MinValue, char.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { char.MaxValue, char.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableCharTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(char).ToString(), SearchOperator.Equals, false },
        new object?[] { null, char.MaxValue.ToString(), SearchOperator.Equals, false },
        new object?[] { default(char), null, SearchOperator.Equals, false },
        new object?[] { char.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(char).ToString(), SearchOperator.NotEquals, true },
        new object?[] { null, char.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(char), null, SearchOperator.NotEquals, true },
        new object?[] { char.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(char).ToString(), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, char.MinValue.ToString(), SearchOperator.Greater, false },
        new object?[] { default(char), null, SearchOperator.Greater, false },
        new object?[] { char.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(char).ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, char.MinValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(char), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { char.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(char).ToString(), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, char.MaxValue.ToString(), SearchOperator.Less, false },
        new object?[] { default(char), null, SearchOperator.Less, false },
        new object?[] { char.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(char).ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, char.MaxValue.ToString(), SearchOperator.LessOrEqual, false },
        new object?[] { default(char), null, SearchOperator.LessOrEqual, false },
        new object?[] { char.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { char.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(char), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { char.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(char), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(CharTestCases))]
    public void ShouldHandleChar(
        char propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Char = propValue };

        SearchRule rule = new
        (
            Name: nameof(obj.Char),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(CharTestCases))]
    [MemberData(nameof(NullableCharTestCases))]
    public void ShouldHandleNullableChar(
        char? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableChar = propValue };

        SearchRule rule = new
        (
            Name: nameof(obj.NullableChar),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }
}