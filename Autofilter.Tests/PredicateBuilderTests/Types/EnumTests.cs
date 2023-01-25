using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class EnumTests
{
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
    public void ShouldHandleEnum(TestEnum objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Enum = objValue };

        SearchRule rule = new(nameof(obj.Enum), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(EnumTestCases))]
    [MemberData(nameof(NullableEnumTestCases))]
    public void ShouldHandleNullableEnum(TestEnum? objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableEnum = objValue };

        SearchRule rule = new(nameof(obj.NullableEnum), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public enum TestEnum { One, Two }

    private class TestClass
    {
        public TestEnum Enum { get; init; }
        public TestEnum? NullableEnum { get; init; }
    }
}