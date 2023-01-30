using FluentAssertions;
using System.Globalization;
using System.Linq.Expressions;
using Autofilter.Processors;
using Autofilter.Rules;

namespace Autofilter.Tests.ProcessorTests.Types;

public class DateTimeOffsetTests
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetTestCases))]
    public void ShouldHandleDateTimeOffset(DateTimeOffset objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { DateTimeOffset = objValue };

        Condition condition = new(nameof(obj.DateTimeOffset), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetTestCases))]
    [MemberData(nameof(NullableDateTimeOffsetTestCases))]
    public void ShouldHandleNullableDateTimeOffset(DateTimeOffset? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDateTimeOffset = objValue };

        Condition condition = new(nameof(obj.NullableDateTimeOffset), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> DateTimeOffsetTestCases => new[]
    {
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.Now, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },

        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.Now, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },

        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { DateTimeOffset.Now, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },

        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.Now, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },

        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { DateTimeOffset.Now, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },

        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.Now, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },

        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.InRange, true },
        new object[] { default(DateTimeOffset), new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.InRange, false },
        new object[] { default(DateTimeOffset), Array.Empty<string?>(), SearchOperator.InRange, false },
    };

    public static IEnumerable<object?[]> NullableDateTimeOffsetTestCases => new[]
    {
        new object?[] { null, new string?[]{ null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Equals, true },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object?[] { null, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object?[] { default(DateTimeOffset), new string?[]{ null }, SearchOperator.Equals, false },
        new object?[] { DateTimeOffset.MaxValue, new string?[]{ null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object?[] { default(DateTimeOffset), new string?[]{ null }, SearchOperator.NotEquals, true },
        new object?[] { DateTimeOffset.MaxValue, new string?[]{ null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[]{ null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Greater, false },
        new object?[] { null, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object?[] { default(DateTimeOffset), new string?[]{ null }, SearchOperator.Greater, false },
        new object?[] { DateTimeOffset.MaxValue, new string?[]{ null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object?[] { default(DateTimeOffset), new string?[]{ null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { DateTimeOffset.MaxValue, new string?[]{ null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.Less, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Less, false },
        new object?[] { null, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object?[] { default(DateTimeOffset), new string?[]{ null }, SearchOperator.Less, false },
        new object?[] { DateTimeOffset.MinValue, new string?[]{ null }, SearchOperator.Less, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object?[] { default(DateTimeOffset), new string?[]{ null }, SearchOperator.LessOrEqual, false },
        new object?[] { DateTimeOffset.MinValue, new string?[]{ null }, SearchOperator.LessOrEqual, false },

        new object?[] { DateTimeOffset.Now, null, SearchOperator.Exists, true },
        new object?[] { default(DateTimeOffset), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { DateTimeOffset.Now, null, SearchOperator.NotExists, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.InRange, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString(CultureInfo.InvariantCulture) }, SearchOperator.InRange, false },
        new object?[] { null, new string?[] { null }, SearchOperator.InRange, true }
    };

    private class TestClass
    {
        public DateTimeOffset DateTimeOffset { get; init; }
        public DateTimeOffset? NullableDateTimeOffset { get; init; }
    }
}
