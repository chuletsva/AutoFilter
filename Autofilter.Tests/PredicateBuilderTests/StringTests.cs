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
    public static IEnumerable<object?[]> TestCases
    {
        get
        {
            // Equals
            yield return new object?[] { Guid.Empty.ToString(), Guid.Empty.ToString(), SearchOperator.Equals, true };
            yield return new object?[] { string.Empty, string.Empty, SearchOperator.Equals, true };
            yield return new object?[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), SearchOperator.Equals, false };

            yield return new object?[] { " ", string.Empty, SearchOperator.Equals, false };
            yield return new object?[] { string.Empty, " ", SearchOperator.Equals, false };

            yield return new object?[] { null, null, SearchOperator.Equals, true };
            yield return new object?[] { null, default(string), SearchOperator.Equals, true };
            yield return new object?[] { null, string.Empty, SearchOperator.Equals, false };
            yield return new object?[] { null, Guid.Empty.ToString(), SearchOperator.Equals, false };
            yield return new object?[] { default(string), null, SearchOperator.Equals, true };
            yield return new object?[] { Guid.Empty.ToString(), null, SearchOperator.Equals, false };   
            
            // NotEquals
            yield return new object?[] { Guid.Empty.ToString(), Guid.Empty.ToString(), SearchOperator.NotEquals, false };
            yield return new object?[] { string.Empty, string.Empty, SearchOperator.NotEquals, false };
            yield return new object?[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), SearchOperator.NotEquals, true };

            yield return new object?[] { " ", string.Empty, SearchOperator.NotEquals, true };
            yield return new object?[] { string.Empty, " ", SearchOperator.NotEquals, true };

            yield return new object?[] { null, null, SearchOperator.NotEquals, false };
            yield return new object?[] { null, default(string), SearchOperator.NotEquals, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotEquals, true };
            yield return new object?[] { null, Guid.Empty.ToString(), SearchOperator.NotEquals, true };
            yield return new object?[] { default(string), null, SearchOperator.NotEquals, false };
            yield return new object?[] { Guid.Empty.ToString(), null, SearchOperator.NotEquals, true };

            // Other
            yield return new object?[] { string.Empty, null, SearchOperator.Exists, true };
            yield return new object?[] { Guid.Empty.ToString(), null, SearchOperator.Exists, true };
            yield return new object?[] { null, null, SearchOperator.Exists, false };

            yield return new object?[] { null, null, SearchOperator.NotExists, true };
            yield return new object?[] { Guid.Empty.ToString(), null, SearchOperator.NotExists, false };

            yield return new object?[] { "ab", "a", SearchOperator.StartsWith, true };
            yield return new object?[] { "ab", "b", SearchOperator.StartsWith, false };
            yield return new object?[] { null, string.Empty, SearchOperator.StartsWith, false };
            yield return new object?[] { string.Empty, null, SearchOperator.StartsWith, false };
            yield return new object?[] { null, null, SearchOperator.StartsWith, false };

            yield return new object?[] { "ab", "b", SearchOperator.EndsWith, true };
            yield return new object?[] { "ab", "a", SearchOperator.EndsWith, false };
            yield return new object?[] { null, string.Empty, SearchOperator.EndsWith, false };
            yield return new object?[] { string.Empty, null, SearchOperator.EndsWith, false };
            yield return new object?[] { null, null, SearchOperator.EndsWith, false };

            yield return new object?[] { "a", "a", SearchOperator.Contains, true };
            yield return new object?[] { "abc", "b", SearchOperator.Contains, true };
            yield return new object?[] { "a", "b", SearchOperator.Contains, false };
            yield return new object?[] { null, string.Empty, SearchOperator.Contains, false };
            yield return new object?[] { string.Empty, null, SearchOperator.Contains, false };
            yield return new object?[] { null, null, SearchOperator.Contains, false };

            yield return new object?[] { "abc", "d", SearchOperator.NotContains, true };
            yield return new object?[] { "abc", "b", SearchOperator.NotContains, false };
            yield return new object?[] { null, string.Empty, SearchOperator.NotContains, false };
            yield return new object?[] { string.Empty, null, SearchOperator.NotContains, false };
            yield return new object?[] { null, null, SearchOperator.NotContains, false };
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
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
}