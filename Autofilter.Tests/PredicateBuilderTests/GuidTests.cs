using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

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
        }
    }

    public static IEnumerable<object?[]> NullableGuidTestCases
    {
        get
        {
            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { null, default(Guid).ToString(), SearchOperator.Equals, false };
            yield return new object?[] { null, Guid.NewGuid().ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(Guid), null, SearchOperator.Equals, false };
            yield return new object?[] { Guid.NewGuid(), null, SearchOperator.Equals, false };

            yield return new object?[] { Guid.NewGuid(), null, SearchOperator.Exists, true };
            yield return new object?[] { default(Guid), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { Guid.NewGuid(), null, SearchOperator.NotExists, false };
            yield return new object?[] { default(Guid), null, SearchOperator.NotExists, false };
        }
    }

    [Theory]
    [MemberData(nameof(GuidTestCases))]
    public void ShouldHandleGuid(
        Guid propValue, string ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { Guid = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.Guid),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GuidTestCases))]
    [MemberData(nameof(NullableGuidTestCases))]
    public void ShouldHandleNullableGuid(
        Guid? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { NullableGuid = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.NullableGuid),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}