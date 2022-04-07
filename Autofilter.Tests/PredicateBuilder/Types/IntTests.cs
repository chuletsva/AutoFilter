using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class IntTests
{
    class TestClass
    {
        public int Int { get; init; }
        public int? NullableInt { get; init; }
    }

    public static IEnumerable<object[]> IntTestCases => new[]
    {
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Equals, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Equals, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Equals, true },    

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { default(int), default(int).ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.NotEquals, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Greater, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Less, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableIntTestCases => new[]
    {
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Equals, false },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Equals, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Equals, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Equals, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Equals, true },    

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.NotEquals, true },
        new object[] { default(int), default(int).ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.NotEquals, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.NotEquals, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Greater, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Greater, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Greater, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Greater, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Greater, false },

        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.GreaterOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.GreaterOrEqual, true },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.Less, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.Less, false },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.Less, false },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.Less, false },

        new object[] { int.MinValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, false },
        new object[] { default(int), default(int).ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MinValue, int.MinValue.ToString(), SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, int.MaxValue.ToString(), SearchOperator.LessOrEqual, true },
    };

    [Theory]
    [MemberData(nameof(IntTestCases))]
    public void ShouldHandleInt(int propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { Int = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Int),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression =
            Helpers.PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(IntTestCases))]
    [MemberData(nameof(NullableIntTestCases))]
    public void ShouldHandleNullableInt(int? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { NullableInt = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableInt),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression =
            Helpers.PredicateBuilder.BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}