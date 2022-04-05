using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Model;
using FluentAssertions;
using Xunit;

namespace Autofilter.Tests.PredicateBuilderTests;

public class LogicOperatorTests
{
    class TestClass
    {
        public bool V1 => true;
        public bool V2 => true;
        public bool V3 => true;
        public bool V4 => true;
        public bool V5 => true;
        public bool V6 => true;
    }

    public static IEnumerable<object[]> TwoOperandsTestCases => new[]
    {
        new object[] { "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "true", false },
        new object[] { "false", LogicOperator.And, "false", false },

        new object[] { "true", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.Or, "false", false },
    };

    public static IEnumerable<object> ThreeOperandsTestCases => new[]
    {
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.And, "true", false },
        new object[] { "true", LogicOperator.And, "false", LogicOperator.And, "true", false },
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "true", false },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.And, "false", false },
        new object[] { "true", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", false },

        new object[] { "true", LogicOperator.And, "true", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.And, "false", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.And, "true", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.Or, "false", false },
        new object[] { "true", LogicOperator.And, "false", LogicOperator.Or, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.Or, "false", false },

        new object[] { "true", LogicOperator.Or, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.Or, "true", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.And, "true", false },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.And, "false", false },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.And, "false", false },

        new object[] { "true", LogicOperator.Or, "true", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.Or, "true", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.Or, "false", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "false", false },
    };

    public static IEnumerable<object[]> SixOperandsTestCases => new[]
    {
        #region 0 or

        // and and and and and 
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "true", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "true", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "true", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "true", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },

        #endregion

        #region 1 or

        /* or and and and and 
         * and or and and and 
         * and and or and and
         * and and and or and
         * and and and and or 
         */
        
        // or and and and and 
        new object[] { "true", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },

        // and or and and and 
        new object[] { "true", LogicOperator.And, "true", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.Or, "true", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },

        // and and or and and
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "true", LogicOperator.Or, "true", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },

        // and and and or and
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.Or, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "true", LogicOperator.Or, "true", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.Or, "false", LogicOperator.And, "false", false },

        // and and and and or 
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.Or, "false", false },
        new object[] { "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.Or, "false", false },

        #endregion

        #region 2 or

        /* or or and and and
         * or and or and and
         * or and and or and
         * or and and and or
         *
         * and or or and and
         * and or and or and
         * and or and and or
         *
         * and and or or and
         * and and or and or
         * and and and or or
         */

        // or or and and and
        new object[] { "true", LogicOperator.Or, "true", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "true", LogicOperator.Or, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "true", LogicOperator.And, "true", LogicOperator.And, "true", LogicOperator.And, "true", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "true", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.And, "false", LogicOperator.And, "false", LogicOperator.And, "false", false },

        // or and or and and

        #endregion

        #region 3 or

        /*
         * or or or and and
         * or or and or and
         * or or and and or
         *
         * or and or and or
         * or and and or or
         *
         * and or or or and
         * and or or and or
         * and or and or or
         * and and or or or
         */
        
        #endregion

        #region 4 or

        /* or or or or and
         * or or or and or
         * or or and or or
         * or and or or or
         * and or or or or
         */

        #endregion

        #region 5 or

        // or or or or or 
        new object[] { "true", LogicOperator.Or, "true", LogicOperator.Or, "true", LogicOperator.Or, "true", LogicOperator.Or, "true", LogicOperator.Or, "true", true },
        new object[] { "true", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "true", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "true", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "true", LogicOperator.Or, "false", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "true", LogicOperator.Or, "false", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "true", true },
        new object[] { "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", LogicOperator.Or, "false", false },

        #endregion
    };

    [Theory]
    [MemberData(nameof(TwoOperandsTestCases))]
    public void ShouldHandleTwoOperands(
        string v1, LogicOperator op, string v2, bool result)
    {
        Expression<Func<TestClass, bool>> expression = 
            PredicateBuilder.BuildPredicate<TestClass>(new SearchRule[]{
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, op)
            });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(new()).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ThreeOperandsTestCases))]
    public void ShouldHandleThreeOperands(
        string v1, LogicOperator op1, 
        string v2, LogicOperator op2, 
        string v3, bool result)
    {
        Expression<Func<TestClass, bool>> expression = 
            PredicateBuilder.BuildPredicate<TestClass>(new SearchRule[]{
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, op1),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, op2),
            });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(new()).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(SixOperandsTestCases))]
    public void ShouldHandleSixOperands(
        string v1, LogicOperator op1, 
        string v2, LogicOperator op2,
        string v3, LogicOperator op3, 
        string v4, LogicOperator op4, 
        string v5, LogicOperator op5,
        string v6, bool result)
    {
        Expression<Func<TestClass, bool>> expression = 
            PredicateBuilder.BuildPredicate<TestClass>(new SearchRule[]{
                new(nameof(TestClass.V1), v1, SearchOperator.Equals),
                new(nameof(TestClass.V2), v2, SearchOperator.Equals, op1),
                new(nameof(TestClass.V3), v3, SearchOperator.Equals, op2),
                new(nameof(TestClass.V4), v4, SearchOperator.Equals, op3),
                new(nameof(TestClass.V5), v5, SearchOperator.Equals, op4),
                new(nameof(TestClass.V6), v6, SearchOperator.Equals, op5),
            });

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(new()).Should().Be(result);
    }
}