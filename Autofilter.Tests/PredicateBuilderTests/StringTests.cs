using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using Autofilter.Tests.FakeData;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class StringTests
{
    [Theory]
    [MemberData(nameof(StringTestCases))]
    public void ShouldHandleString(
        string? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        PropTypesTestClass obj = new() { String = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.String),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<PropTypesTestClass, bool>> expression =
            PredicateBuilder.BuildPredicate<PropTypesTestClass>(new[] { rule });

        Func<PropTypesTestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }

    public static IEnumerable<object?[]> StringTestCases
    {
        get
        {
            yield return new object?[] { Guid.Empty.ToString(), Guid.Empty.ToString(), SearchOperator.Equals, true };
            yield return new object?[] { string.Empty, string.Empty, SearchOperator.Equals, true };

            yield return new object?[] { "  ", string.Empty, SearchOperator.Equals, false };
            yield return new object?[] { string.Empty, "  ", SearchOperator.Equals, false };

            yield return new object?[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { string.Empty, null, SearchOperator.Equals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, false };
            yield return new object?[] { Guid.NewGuid().ToString(), null, SearchOperator.Equals, false };
            yield return new object?[] { null, Guid.NewGuid().ToString(), SearchOperator.Equals, false };
        }
    }
}