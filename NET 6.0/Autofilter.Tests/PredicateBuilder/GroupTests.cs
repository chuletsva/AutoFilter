using System.Linq.Expressions;
using Autofilter.Models;
using FluentAssertions;
using static Autofilter.Helpers.PredicateBuilder;

namespace Autofilter.Tests.PredicateBuilder;

public class GroupTests
{
    public class SimpleScenarios
    {
        // (1 or 2 and 3)
        [Theory]
        [InlineData("true", "true", "false", true)]
        [InlineData("false", "true", "true", true)]
        [InlineData("false", "true", "false", false)]
        public void SingleGroup_IncludeBorders(string prop1, string prop2, string prop3, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups = { new(Start: 1, End: search.Length, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        // 1 and (2 or 3 or 4) and 5
        [Theory]
        [InlineData("true", "false", "true", "false", "true", true)]
        [InlineData("false", "false", "true", "false", "true", false)]
        public void SingleGroup_DoesntIncludeBorders(
            string prop1, string prop2, 
            string prop3, string prop4, 
            string prop5, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop5), prop5, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups = { new(Start: 2, End: 4, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        // (1 and 2 or 3) and 4
        [Theory]
        [InlineData("true", "true", "false", "true", true)]
        [InlineData("false", "false", "true", "true", true)]
        [InlineData("true", "false", "false", "false", false)]
        [InlineData("false", "true", "false", "false", false)]
        public void SingleGroup_IncludeStartBorder(
            string prop1, string prop2, string prop3, 
            string prop4, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups = { new(Start: 1, End: 3, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        // 1 and (2 and 3 or 4)
        [Theory]
        [InlineData("true", "false", "false", "true", true)]
        [InlineData("true", "true", "true", "false", true)]
        [InlineData("true", "true", "false", "false", false)]
        public void SingleGroup_IncludeEndBorder(
            string prop1, string prop2, string prop3, 
            string prop4, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, LogicOperator.Or),
            };

            GroupRule[] groups = { new(Start: 2, End: 4, Level: 1) };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        // (1 or 2 and 3) and (4 and 5 or 6)
        [Theory]
        [InlineData("true", "false", "false", "false", "false", "true", true)]
        [InlineData("false", "true", "true", "true", "true", "false", true)]
        [InlineData("false", "true", "false", "false", "true", "false", false)]
        [InlineData("false", "false", "true", "true", "false", "false", false)]
        public void TwoGroups_IncludeBorders(
            string prop1, string prop2, string prop3, 
            string prop4, string prop5, string prop6, 
            bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop5), prop5, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop6), prop6, SearchOperator.Equals, LogicOperator.Or),
            };

            GroupRule[] groups =
            {
                new(Start: 1, End: 3, Level: 1),
                new(Start: 4, End: 6, Level: 1)
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        // 1 and (2 or 3 and 4) or (5 and 6 or 7) and 8
        [Theory]
        [InlineData("true", "false", "true", "true", "true", "true", "false", "true", true)]
        [InlineData("true", "true", "false", "false", "false", "false", "false", "false", true)]
        [InlineData("false", "false", "false", "false", "true", "true", "false", "true", true)]
        [InlineData("false", "false", "false", "false", "false", "false", "true", "true", true)]
        [InlineData("false", "true", "true", "true", "true", "true", "true", "false", false)]
        [InlineData("true", "false", "false", "true", "true", "false", "false", "true", false)]
        public void TwoGroups_BordersNotIncluded(
            string prop1, string prop2, string prop3, 
            string prop4, string prop5, string prop6, 
            string prop7, string prop8, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop5), prop5, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop6), prop6, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop7), prop7, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop8), prop8, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups =
            {
                new(Start: 2, End: 4, Level: 1),
                new(Start: 5, End: 7, Level: 1)
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }
    }

    public class ComplexScenarions
    {
        [Theory]
        // 1 and ((2 or 3) and (4 or 5)) and 6
        [InlineData("true", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "true", true)]
        // 1 or ((2 and 3) or (4 and 6)) or 6
        [InlineData("false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.Or, "false", true)]
        public void ThreeGroups_BordersNotIncluded(
            string prop1, LogicOperator logic1, 
            string prop2, LogicOperator logic2, 
            string prop3, LogicOperator logic3,
            string prop4,  LogicOperator logic4, 
            string prop5,  LogicOperator logic5,
            string prop6, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, logic1),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, logic2),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, logic3),
                new(nameof(TestClass.Prop5), prop5, SearchOperator.Equals, logic4),
                new(nameof(TestClass.Prop6), prop6, SearchOperator.Equals, logic5),
            };

            GroupRule[] groups =
            {
                new(Start: 2, End: 3, Level: 1),
                new(Start: 4, End: 5, Level: 1),
                new(Start: 2, End: 5, Level: 2),
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        [Theory]
        // ((1 and 2) or (3 and 4)) and 5
        [InlineData("true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "true", true)]
        // ((1 or 2) and (3 or 4)) and 5
        [InlineData("true", LogicOperator.Or, "false", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "true", true)]
        public void ThreeGroups_IncludeStartBorder(
            string prop1, LogicOperator logic1, 
            string prop2, LogicOperator logic2, 
            string prop3, LogicOperator logic3,
            string prop4, LogicOperator logic4, 
            string prop5, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, logic1),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, logic2),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, logic3),
                new(nameof(TestClass.Prop5), prop5, SearchOperator.Equals, logic4),
            };

            GroupRule[] groups =
            {
                new(Start: 1, End: 2, Level: 1),
                new(Start: 3, End: 4, Level: 1),
                new(Start: 1, End: 4, Level: 2),
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }

        // (1 and 2 or 3) and (4 or (5 and 6 or 7) and (8 or 9 and 10) or 11) and (12 or 13 and 14)
        [Theory]
        [InlineData("true", "true", "false", "true", "false", "false", "false", "false", "false", "false", "false", "true", "false", "false", true)]
        [InlineData("false", "false", "true", "false", "true", "true", "false", "false", "true", "true", "false", "false", "true", "true", true)]
        public void FiveGroups_IncludeBorders(
            string prop1, string prop2, string prop3, 
            string prop4, string prop5, string prop6, 
            string prop7, string prop8, string prop9,
            string prop10, string prop11, string prop12,
            string prop13, string prop14, bool result)
        {
            SearchRule[] search = 
            {
                new(nameof(TestClass.Prop1), prop1, SearchOperator.Equals),
                new(nameof(TestClass.Prop2), prop2, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop3), prop3, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop4), prop4, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop5), prop5, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop6), prop6, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop7), prop7, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop8), prop8, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop9), prop9, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop10), prop10, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop11), prop11, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop12), prop12, SearchOperator.Equals, LogicOperator.And),
                new(nameof(TestClass.Prop13), prop13, SearchOperator.Equals, LogicOperator.Or),
                new(nameof(TestClass.Prop14), prop14, SearchOperator.Equals, LogicOperator.And),
            };

            GroupRule[] groups =
            {
                new(Start: 1, End: 3, Level: 2),
                new(Start: 4, End: 11, Level: 2),
                new(Start: 12, End: 14, Level: 2),
                new(Start: 5, End: 7, Level: 1),
                new(Start: 8, End: 10, Level: 1)
            };

            Expression<Func<TestClass, bool>> expression = BuildPredicate<TestClass>(search, groups);

            Func<TestClass, bool> predicate = expression.Compile();

            predicate(new()).Should().Be(result);
        }
    }

    private class TestClass
    {
        public bool Prop1 => true;
        public bool Prop2 => true;
        public bool Prop3 => true;
        public bool Prop4 => true;
        public bool Prop5 => true;
        public bool Prop6 => true;
        public bool Prop7 => true;
        public bool Prop8 => true;
        public bool Prop9 => true;
        public bool Prop10 => true;
        public bool Prop11 => true;
        public bool Prop12 => true;
        public bool Prop13 => true;
        public bool Prop14 => true;
    }
}