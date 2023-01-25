using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class IntTests
{
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
    public void ShouldHandleInt(int objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Int = objValue };

        SearchRule rule = new(nameof(obj.Int), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(IntTestCases))]
    [MemberData(nameof(NullableIntTestCases))]
    public void ShouldHandleNullableInt(int? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableInt = objValue };

        SearchRule rule = new(nameof(obj.NullableInt), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    private class TestClass
    {
        public int Int { get; init; }
        public int? NullableInt { get; init; }
    }
}