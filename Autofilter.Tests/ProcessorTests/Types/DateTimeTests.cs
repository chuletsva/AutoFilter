using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;
using FluentAssertions;

namespace Autofilter.Tests.ProcessorTests.Types;

public class DateTimeTests
{
    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    public void ShouldHandleDateTime(DateTime objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { DateTime = objValue };

        Condition condition = new(nameof(obj.DateTime), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    [MemberData(nameof(NullableDateTimeTestCases))]
    public void ShouldHandleNullableDateTime(DateTime? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDateTime = objValue };

        Condition condition = new(nameof(obj.NullableDateTime), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object?[]> DateTimeTestCases => new[]
    {
        new object[] { default(DateTime), new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTime.Now, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },

        new object[] { default(DateTime), new[]{ default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTime.Now, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },

        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { DateTime.Now, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },

        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.Now, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },

        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { DateTime.Now, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },

        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.Now, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },

        new object[] { default(DateTime), new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.InRange, true },
        new object[] { default(DateTime), new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.InRange, false },
        new object[] { default(DateTime), Array.Empty<string?>(), SearchOperator.InRange, false },
    };

    public static IEnumerable<object?[]> NullableDateTimeTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Equals, true },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object?[] { null, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { DateTime.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Greater, false },
        new object?[] { null, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { DateTime.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Less, false },
        new object?[] { null, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.Less, false },
        new object?[] { DateTime.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { DateTime.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { DateTime.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { DateTime.Now, null, SearchOperator.Exists, true },
        new object?[] { default(DateTime), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { DateTime.Now, null, SearchOperator.NotExists, false },
        new object?[] { default(DateTime), null, SearchOperator.NotExists, false },

        
        new object?[] { null, Array.Empty<string?>(), SearchOperator.InRange, false },
        new object?[] { null, new[] { default(DateTime).ToString(CultureInfo.InvariantCulture) }, SearchOperator.InRange, false },
        new object?[] { null, new string?[] { null }, SearchOperator.InRange, true }
    };

    private class TestClass
    {
        public DateTime DateTime { get; init; }
        public DateTime? NullableDateTime { get; init; }
    }
}