using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder.Types;

public class StringTests
{
    class TestClass
    {
        public string? String { get; init; }
    }

    public static IEnumerable<object?[]> TestCases => new[]
    {
        // Equals
        new object?[] { Guid.Empty.ToString(), Guid.Empty.ToString(), SearchOperator.Equals, true },
        new object?[] { string.Empty, string.Empty, SearchOperator.Equals, true },
        new object?[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), SearchOperator.Equals, false },

        new object?[] { " ", string.Empty, SearchOperator.Equals, false },
        new object?[] { string.Empty, " ", SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, default(string), SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, false },
        new object?[] { null, Guid.Empty.ToString(), SearchOperator.Equals, false },
        new object?[] { default(string), null, SearchOperator.Equals, true },
        new object?[] { Guid.Empty.ToString(), null, SearchOperator.Equals, false },   
        
        // NotEquals
        new object?[] { Guid.Empty.ToString(), Guid.Empty.ToString(), SearchOperator.NotEquals, false },
        new object?[] { string.Empty, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), SearchOperator.NotEquals, true },

        new object?[] { " ", string.Empty, SearchOperator.NotEquals, true },
        new object?[] { string.Empty, " ", SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, default(string), SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, true },
        new object?[] { null, Guid.Empty.ToString(), SearchOperator.NotEquals, true },
        new object?[] { default(string), null, SearchOperator.NotEquals, false },
        new object?[] { Guid.Empty.ToString(), null, SearchOperator.NotEquals, true },

        // Other
        new object?[] { string.Empty, null, SearchOperator.Exists, true },
        new object?[] { Guid.Empty.ToString(), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { Guid.Empty.ToString(), null, SearchOperator.NotExists, false },

        new object?[] { "ab", "a", SearchOperator.StartsWith, true },
        new object?[] { "ab", "b", SearchOperator.StartsWith, false },
        new object?[] { null, string.Empty, SearchOperator.StartsWith, false },
        new object?[] { string.Empty, null, SearchOperator.StartsWith, false },
        new object?[] { null, null, SearchOperator.StartsWith, false },

        new object?[] { "ab", "b", SearchOperator.EndsWith, true },
        new object?[] { "ab", "a", SearchOperator.EndsWith, false },
        new object?[] { null, string.Empty, SearchOperator.EndsWith, false },
        new object?[] { string.Empty, null, SearchOperator.EndsWith, false },
        new object?[] { null, null, SearchOperator.EndsWith, false },

        new object?[] { "a", "a", SearchOperator.Contains, true },
        new object?[] { "abc", "b", SearchOperator.Contains, true },
        new object?[] { "a", "b", SearchOperator.Contains, false },
        new object?[] { null, string.Empty, SearchOperator.Contains, false },
        new object?[] { string.Empty, null, SearchOperator.Contains, false },
        new object?[] { null, null, SearchOperator.Contains, false },

        new object?[] { "abc", "d", SearchOperator.NotContains, true },
        new object?[] { "abc", "b", SearchOperator.NotContains, false },
        new object?[] { null, string.Empty, SearchOperator.NotContains, false },
        new object?[] { string.Empty, null, SearchOperator.NotContains, false },
        new object?[] { null, null, SearchOperator.NotContains, false },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldHandleString(
        string? propValue, string? ruleValue, 
        SearchOperator operation, bool result)
    {
        TestClass obj = new() { String = propValue };

        SearchRule rule = new
        (
            PropertyName: nameof(obj.String),
            Value: ruleValue,
            SearchOperator: operation
        );

        Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(new[] { rule });

        Func<TestClass, bool> func = expression.Compile();

        func(obj).Should().Be(result);
    }
}