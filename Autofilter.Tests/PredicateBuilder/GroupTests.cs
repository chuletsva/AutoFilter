using System;
using System.Linq.Expressions;
using Autofilter.Model;
using FluentAssertions;
using Xunit;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder;

public class GroupTests
{
    class TestClass
    {
        public bool V1 => true;
        public bool V2 => true;
        public bool V3 => true;
        public bool V4 => true;
        public bool V5 => true;
        public bool V6 => true;
        public bool V7 => true;
        public bool V8 => true;
        public bool V9 => true;
        public bool V10 => true;
        public bool V11 => true;
        public bool V12 => true;
        public bool V13 => true;
        public bool V14 => true;
    }

    public class SimpleScenarios
    {
        // (* or * and *)
        [Theory]
        [InlineData("true", "true", "false", true)]
        [InlineData("false", "true", "true", true)]
        [InlineData("false", "true", "false", false)]
        public void SingleGroup_IncludeBorders(string v1, string v2, string v3, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups = { new(Start: 0, End: search.Length - 1, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }

        // * and (* or * or *) and *
        [Theory]
        [InlineData("true", "false", "true", "false", "true", true)]
        [InlineData("false", "false", "true", "false", "true", false)]
        public void SingleGroup_DoesntIncludeBorders(
            string v1, string v2, 
            string v3, string v4, 
            string v5, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups = { new(Start: 1, End: 3, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }

        // (* and * or *) and *
        [Theory]
        [InlineData("true", "true", "false", "true", true)]
        [InlineData("false", "false", "true", "true", true)]
        [InlineData("true", "false", "false", "false", false)]
        [InlineData("false", "true", "false", "false", false)]
        public void SingleGroup_IncludeStartBorder(
            string v1, string v2, string v3, 
            string v4, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups = { new(Start: 0, End: 2, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }

        // * and (* and * or *)
        [Theory]
        [InlineData("true", "false", "false", "true", true)]
        [InlineData("true", "true", "true", "false", true)]
        [InlineData("true", "true", "false", "false", false)]
        public void SingleGroup_IncludeEndBorder(
            string v1, string v2, string v3, 
            string v4, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, LogicOperator.Or),
            };

            GroupRule[] groups = { new(Start: 1, End: 3, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }

        // (* or * and *) and (* and * or *)
        [Theory]
        [InlineData("true", "false", "false", "false", "false", "true", true)]
        [InlineData("false", "true", "true", "true", "true", "false", true)]
        [InlineData("false", "true", "false", "false", "true", "false", false)]
        [InlineData("false", "false", "true", "true", "false", "false", false)]
        public void TwoGroups_IncludeBorders(
            string v1, string v2, string v3, 
            string v4, string v5, string v6, 
            bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V6), v6, SearchOperator.Equals, LogicOperator.Or),
            };

            GroupRule[] groups =
            {
                new(Start: 0, End: 2, Level: 1),
                new(Start: 3, End: 5, Level: 1)
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }

        // * and (* or * and *) or (* and * or *) and *
        [Theory]
        [InlineData("true", "false", "true", "true", "true", "true", "false", "true", true)]
        [InlineData("true", "true", "false", "false", "false", "false", "false", "false", true)]
        [InlineData("false", "false", "false", "false", "true", "true", "false", "true", true)]
        [InlineData("false", "false", "false", "false", "false", "false", "true", "true", true)]
        [InlineData("false", "true", "true", "true", "true", "true", "true", "false", false)]
        [InlineData("true", "false", "false", "true", "true", "false", "false", "true", false)]
        public void TwoGroups_DoesntIncludeBorders(
            string v1, string v2, string v3, 
            string v4, string v5, string v6, 
            string v7, string v8, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V6), v6, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V7), v7, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V8), v8, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups =
            {
                new(Start: 1, End: 3, Level: 1),
                new(Start: 4, End: 6, Level: 1)
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }
    }

    public class ComplexScenarions
    {
        [Theory]
        // * and ((* or *) and (* or *)) and *
        [InlineData("true", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "true", true)]
        // * or ((* and *) or (* and *)) or *
        [InlineData("false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.Or, "false", true)]
        public void ThreeGroups_DoesntIncludeBorders(
            string v1, LogicOperator logic1, 
            string v2, LogicOperator logic2, 
            string v3, LogicOperator logic3,
            string v4,  LogicOperator logic4, 
            string v5,  LogicOperator logic5,
            string v6, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, logic1),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, logic2),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, logic3),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, logic4),
                new(nameof(TestClass.V6), v6, SearchOperator.Equals, logic5),
            };

            GroupRule[] groups =
            {
                new(Start: 1, End: 2, Level: 1),
                new(Start: 3, End: 4, Level: 1),
                new(Start: 1, End: 4, Level: 2),
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }
        [Theory]
        // ((* and *) or (* and *)) and *
        [InlineData("true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "true", true)]
        // ((* or *) and (* or *)) and *
        [InlineData("true", LogicOperator.Or, "false", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "true", true)]
        public void ThreeGroups_IncludeStartBorder(
            string v1, LogicOperator logic1, 
            string v2, LogicOperator logic2, 
            string v3, LogicOperator logic3,
            string v4, LogicOperator logic4, 
            string v5, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, logic1),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, logic2),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, logic3),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, logic4),
            };

            GroupRule[] groups =
            {
                new(Start: 0, End: 1, Level: 1),
                new(Start: 2, End: 3, Level: 1),
                new(Start: 0, End: 3, Level: 2),
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }

        // (* and * or *) and (* or (* and * or *) and (* or * and *) or *) and (* or * and *)
        [Theory]
        [InlineData("true", "true", "false", "true", "false", "false", "false", "false", "false", "false", "false", "true", "false", "false", true)]
        [InlineData("false", "false", "true", "false", "true", "true", "false", "false", "true", "true", "false", "false", "true", "true", true)]
        public void FiveGroups_IncludeBorders(
            string v1, string v2, string v3, 
            string v4, string v5, string v6, 
            string v7, string v8, string v9,
            string v10, string v11, string v12,
            string v13, string v14, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V6), v6, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V7), v7, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V8), v8, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V9), v9, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V10), v10, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V11), v11, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V12), v12, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.V13), v13, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.V14), v14, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups =
            {
                new(Start: 0, End: 2, Level: 1),
                new(Start: 4, End: 6, Level: 1),
                new(Start: 7, End: 9, Level: 1),
                new(Start: 11, End: 13, Level: 1),
                new(Start: 3, End: 10, Level: 2)
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> func = expression.Compile();

            func(new()).Should().Be(result);
        }
    }
}