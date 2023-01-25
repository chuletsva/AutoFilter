using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class GuidTests
{
    public static IEnumerable<object[]> GuidTestCases
    {
        get
        {
            Guid guid = Guid.NewGuid();

            yield return new object[] { guid, guid.ToString(), SearchOperator.Equals, true };
            yield return new object[] { Guid.Empty, Guid.Empty.ToString(), SearchOperator.Equals, true };
            yield return new object[] { Guid.NewGuid(), Guid.NewGuid().ToString(), SearchOperator.Equals, false };

            yield return new object[] { guid, guid.ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { Guid.Empty, Guid.Empty.ToString(), SearchOperator.NotEquals, false };
            yield return new object[] { Guid.NewGuid(), Guid.NewGuid().ToString(), SearchOperator.NotEquals, true };
        }
    }

    public static IEnumerable<object?[]> NullableGuidTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(Guid).ToString(), SearchOperator.Equals, false },
        new object?[] { null, Guid.NewGuid().ToString(), SearchOperator.Equals, false },
        new object?[] { default(Guid), null, SearchOperator.Equals, false },
        new object?[] { Guid.NewGuid(), null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(Guid).ToString(), SearchOperator.NotEquals, true },
        new object?[] { null, Guid.NewGuid().ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(Guid), null, SearchOperator.NotEquals, true },
        new object?[] { Guid.NewGuid(), null, SearchOperator.NotEquals, true },

        new object?[] { Guid.NewGuid(), null, SearchOperator.Exists, true },
        new object?[] { default(Guid), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { Guid.NewGuid(), null, SearchOperator.NotExists, false },
        new object?[] { default(Guid), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(GuidTestCases))]
    public void ShouldHandleGuid(Guid objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Guid = objValue };

        SearchRule rule = new(nameof(obj.Guid), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GuidTestCases))]
    [MemberData(nameof(NullableGuidTestCases))]
    public void ShouldHandleNullableGuid(Guid? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableGuid = objValue };

        SearchRule rule = new(nameof(obj.NullableGuid), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    private class TestClass
    {
        public Guid Guid { get; init; }
        public Guid? NullableGuid { get; init; }
    }
}