using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests.Types;

public class DateTimeTests
{
    public static IEnumerable<object?[]> DateTimeTestCases => new[]
    {
        new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false }, 

        new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },

        new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },

        new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },

        new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },

        new object[] { DateTime.MinValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MaxValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { default(DateTime), default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTime.Now.Date, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MinValue.Date, DateTime.MinValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MaxValue.Date, DateTime.MaxValue.Date.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableDateTimeTestCases => new[]
    { 
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { null, DateTime.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { default(DateTime), null, SearchOperator.Equals, false },
        new object?[] { DateTime.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { null, DateTime.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { default(DateTime), null, SearchOperator.NotEquals, true },
        new object?[] { DateTime.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, DateTime.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { default(DateTime), null, SearchOperator.Greater, false },
        new object?[] { DateTime.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, DateTime.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(DateTime), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { DateTime.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, DateTime.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { default(DateTime), null, SearchOperator.Less, false },
        new object?[] { DateTime.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(DateTime).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, DateTime.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { default(DateTime), null, SearchOperator.LessOrEqual, false },
        new object?[] { DateTime.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { DateTime.Now, null, SearchOperator.Exists, true },
        new object?[] { default(DateTime), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { DateTime.Now, null, SearchOperator.NotExists, false },
        new object?[] { default(DateTime), null, SearchOperator.NotExists, false },
    };

    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    public void ShouldHandleDateTime(DateTime objValue, string searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { DateTime = objValue };

        SearchRule rule = new(nameof(obj.DateTime), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    [MemberData(nameof(NullableDateTimeTestCases))]
    public void ShouldHandleNullableDateTime(DateTime? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDateTime = objValue };

        SearchRule rule = new(nameof(obj.NullableDateTime), searchValue, searchOperator);

        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(new[] { rule });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    private class TestClass
    {
        public DateTime DateTime { get; init; }
        public DateTime? NullableDateTime { get; init; }
    }
}