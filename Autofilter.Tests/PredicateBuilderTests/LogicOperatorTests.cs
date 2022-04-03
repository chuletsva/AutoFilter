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
        public bool FirstOperand { get; set; }
        public bool SecondOperand { get; set; }
    }

    public static IEnumerable<object[]> TestCases
    {
        get
        {
            yield return new object[] { "true", "true", LogicOperator.And, true };
            yield return new object[] { "true", "false", LogicOperator.And, false };
            yield return new object[] { "false", "true", LogicOperator.And, false };
            yield return new object[] { "false", "false", LogicOperator.And, false };

            yield return new object[] { "true", "true", LogicOperator.Or, true };
            yield return new object[] { "true", "false", LogicOperator.Or, true };
            yield return new object[] { "false", "true", LogicOperator.Or, true };
            yield return new object[] { "false", "false", LogicOperator.Or, false };
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldHandleOperator(
        string firstOperand, string secondOperand, 
        LogicOperator logicOperator, bool result)
    {
        TestClass obj = new() { FirstOperand = true, SecondOperand = true };

        SearchRule[] operands = 
        {
            new
            (
                PropertyName: nameof(obj.FirstOperand),
                Value: firstOperand,
                SearchOperator: SearchOperator.Equals
            ),
            new
            (
                PropertyName: nameof(obj.SecondOperand),
                Value: secondOperand,
                SearchOperator: SearchOperator.Equals,
                LogicOperator: logicOperator
            ),
        };

        Expression<Func<TestClass, bool>> expression = 
            PredicateBuilder.BuildPredicate<TestClass>(operands);

        Func<TestClass, bool> predicate = expression.Compile();

        predicate(obj).Should().Be(result);
    }
}