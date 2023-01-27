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
    public void ShouldHandleDateTimeOffset(DateTimeOffset objValue, string searchValue, SearchOperator searchOperator, bool result)
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
    public void ShouldHandleNullableDateTimeOffset(DateTimeOffset? objValue, string? searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDateTimeOffset = objValue };

        Condition condition = new(nameof(obj.NullableDateTimeOffset), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)FilterProcessor.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> DateTimeOffsetTestCases => new[]
    {
        new object[] { default(DateTimeOffset), default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTimeOffset.Now, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },

        new object[] { default(DateTimeOffset), default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.Now, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },

        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, true },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { default(DateTimeOffset), default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { DateTimeOffset.Now, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },

        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object[] { default(DateTimeOffset), default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.Now, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, true },

        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, true },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { default(DateTimeOffset), default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { DateTimeOffset.Now, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },

        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object[] { default(DateTimeOffset), default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.Now, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MinValue, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, true },
    };

    public static IEnumerable<object?[]> NullableDateTimeOffsetTestCases => new[]
    {
        new object?[] { null, null, SearchOperator.Equals, true },
        new object?[] { null, string.Empty, SearchOperator.Equals, true },
        new object?[] { null, default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { null, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.Equals, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.Equals, false },
        new object?[] { DateTimeOffset.MaxValue, null, SearchOperator.Equals, false },

        new object?[] { null, null, SearchOperator.NotEquals, false },
        new object?[] { null, string.Empty, SearchOperator.NotEquals, false },
        new object?[] { null, default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { null, DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture), SearchOperator.NotEquals, true },
        new object?[] { default(DateTimeOffset), null, SearchOperator.NotEquals, true },
        new object?[] { DateTimeOffset.MaxValue, null, SearchOperator.NotEquals, true },

        new object?[] { null, null, SearchOperator.Greater, false },
        new object?[] { null, default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { null, string.Empty, SearchOperator.Greater, false },
        new object?[] { null, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Greater, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.Greater, false },
        new object?[] { DateTimeOffset.MaxValue, null, SearchOperator.Greater, false },

        new object?[] { null, null, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, DateTimeOffset.MinValue.ToString(CultureInfo.InvariantCulture), SearchOperator.GreaterOrEqual, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.GreaterOrEqual, false },
        new object?[] { DateTimeOffset.MaxValue, null, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, null, SearchOperator.Less, false },
        new object?[] { null, default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { null, string.Empty, SearchOperator.Less, false },
        new object?[] { null, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.Less, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.Less, false },
        new object?[] { DateTimeOffset.MinValue, null, SearchOperator.Less, false },

        new object?[] { null, null, SearchOperator.LessOrEqual, false },
        new object?[] { null, default(DateTimeOffset).ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { null, string.Empty, SearchOperator.LessOrEqual, false },
        new object?[] { null, DateTimeOffset.MaxValue.ToString(CultureInfo.InvariantCulture), SearchOperator.LessOrEqual, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.LessOrEqual, false },
        new object?[] { DateTimeOffset.MinValue, null, SearchOperator.LessOrEqual, false },

        new object?[] { DateTimeOffset.Now, null, SearchOperator.Exists, true },
        new object?[] { default(DateTimeOffset), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { DateTimeOffset.Now, null, SearchOperator.NotExists, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.NotExists, false },
    };

    private class TestClass
    {
        public DateTimeOffset DateTimeOffset { get; init; }
        public DateTimeOffset? NullableDateTimeOffset { get; init; }
    }
}
