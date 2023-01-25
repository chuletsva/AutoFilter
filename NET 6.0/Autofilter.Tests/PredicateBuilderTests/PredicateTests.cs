using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Models;
using Autofilter.Tests.Common;
using FluentAssertions;

namespace Autofilter.Tests.PredicateBuilderTests;

public class PredicateTests
{
    public static IEnumerable<object[]> TestCases
    {
        get
        {
            // 2 operands

            yield return new object[]
            {
                "1 and 2",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                },

                Array.Empty<GroupRule>(),
            }; // 1 and 2

            yield return new object[]
            {
                "1 or 2",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                Array.Empty<GroupRule>(),
            }; // 1 or 2

            // 3 operands

            yield return new object[]
            {
                "(1 and 2) and 3",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                },

                Array.Empty<GroupRule>(),
            }; // 1 and 2 and 3

            yield return new object[]
            {
                "(1 or 2) or 3",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                Array.Empty<GroupRule>(),
            }; // 1 or 2 or 3

            yield return new object[]
            {
                "(1 and 2) or 3",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                Array.Empty<GroupRule>(),
            }; // 1 and 2 or 3

            yield return new object[]
            {
                "1 or (2 and 3)",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                },

                Array.Empty<GroupRule>(),
            }; // 1 or 2 and 3

            // 4 operands

            yield return new object[]
            {
                "((1 and 2) and 3) and 4",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.And),
                },

                Array.Empty<GroupRule>(),
            }; // 1 and 2 and 3 and 4

            yield return new object[]
            {
                "((1 or 2) or 3) or 4",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                Array.Empty<GroupRule>(),
            }; // 1 or 2 or 3 or 4

            yield return new object[]
            {
                "((1 and 2) or 3) and 4",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.And),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 1,
                        End: 3,
                        Level: 1
                    )
                }
            }; // (1 and 2 or 3) and 4

            yield return new object[]
            {
                "1 and ((2 and 3) or 4)",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 2,
                        End: 4,
                        Level: 1
                    )
                }
            }; // 1 and (2 and 3 or 4)

            // 5 operands

            yield return new object[]
            {
                "(1 and ((2 or 3) or 4)) and 5",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.And),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 2,
                        End: 4,
                        Level: 1
                    )
                }
            }; // 1 and (2 or 3 or 4) and 5

            yield return new object[]
            {
                "((1 and 2) or (3 and 4)) or 5",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 1,
                        End: 2,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 3,
                        End: 4,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 1,
                        End: 4,
                        Level: 2
                    )
                }
            }; // ((1 and 2) or (3 and 4)) or 5

            yield return new object[]
            {
                "((1 or 2) and (3 or 4)) and 5",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.And),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 1,
                        End: 2,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 3,
                        End: 4,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 1,
                        End: 4,
                        Level: 2
                    )
                }
            }; // ((1 or 2) and (3 or 4)) and 5

            // 6 operands

            yield return new object[]
            {
                "(1 or (2 and 3)) or ((4 and 5) or 6)",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop6), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 1,
                        End: 3,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 4,
                        End: 6,
                        Level: 1
                    ),
                }
            }; // (1 or 2 and 3) or (4 and 5 or 6)

            yield return new object[]
            {
                "(1 and ((2 or 3) and (4 or 5))) and 6",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop6), "true", SearchOperator.Equals, LogicOperator.And),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 2,
                        End: 3,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 4,
                        End: 5,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 2,
                        End: 5,
                        Level: 2
                    ),
                }
            }; // 1 and ((2 or 3) and (4 or 5)) and 6

            yield return new object[]
            {
                "(1 or ((2 and 3) or (4 and 5))) or 6",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop6), "true", SearchOperator.Equals, LogicOperator.Or),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 2,
                        End: 3,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 4,
                        End: 5,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 2,
                        End: 5,
                        Level: 2
                    ),
                }
            }; // 1 or ((2 and 3) or (4 and 5)) or 6

            // 8 operands

            yield return new object[]
            {
                "(1 and (2 or (3 and 4))) or (((5 and 6) or 7) and 8)",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop6), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop7), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop8), "true", SearchOperator.Equals, LogicOperator.And),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 2,
                        End: 4,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 5,
                        End: 7,
                        Level: 1
                    ),
                }
            }; // 1 and (2 or 3 and 4) or (5 and 6 or 7) and 8

            yield return new object[]
            {
                "((1 and (2 or 3)) and 4) or ((5 and (6 or 7)) and 8)",

                new[]
                {
                    new SearchRule(nameof(TestClass.Prop1), "true", SearchOperator.Equals),
                    new SearchRule(nameof(TestClass.Prop2), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop3), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop4), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop5), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop6), "true", SearchOperator.Equals, LogicOperator.And),
                    new SearchRule(nameof(TestClass.Prop7), "true", SearchOperator.Equals, LogicOperator.Or),
                    new SearchRule(nameof(TestClass.Prop8), "true", SearchOperator.Equals, LogicOperator.And),
                },

                new []
                {
                    new GroupRule
                    (
                        Start: 2,
                        End: 3,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 6,
                        End: 7,
                        Level: 1
                    ),
                    new GroupRule
                    (
                        Start: 1,
                        End: 4,
                        Level: 2
                    ),
                    new GroupRule
                    (
                        Start: 5,
                        End: 8,
                        Level: 2
                    ),
                }
            }; // (1 and (2 or 3) and 4) or (5 and (6 or 7) and 8)
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldGenerateValidExpression(string expectedExpression, SearchRule[] search, GroupRule[] groups)
    {
        Expression<Func<TestClass, bool>> lambda = PredicateBuilder.Build<TestClass>(search, groups);

        string resultExpression = ExpressionPrettifier.PrettifyLambda(lambda);

        resultExpression.Should().Be(expectedExpression);
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
    }
}