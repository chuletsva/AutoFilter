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
    class Product
    {
        public Guid Id { get; set; }
        public int InStock { get; set; }
        public bool IsForSale { get; set; }
    }

    [Theory]
    [MemberData(nameof(AndTestCases))]
    public void ShouldHandleOperatorAnd(string id, string isForSale, bool result)
    {
        Product product = new() { Id = Guid.Empty, IsForSale = true };

        // x => x.Id == id && x.IsForSale == isForSale

        SearchRule[] operands = 
        {
            new
            (
                PropertyName: nameof(product.Id),
                Value: id,
                SearchOperator: SearchOperator.Equals
            ),
            new
            (
                PropertyName: nameof(product.IsForSale),
                Value: isForSale,
                SearchOperator: SearchOperator.Equals,
                LogicOperator: LogicOperator.And
            ),
        };

        Expression<Func<Product, bool>> expression = PredicateBuilder.BuildPredicate<Product>(operands);

        Func<Product, bool> predicate = expression.Compile();

        predicate(product).Should().Be(result);
    }

    public static IEnumerable<object[]> AndTestCases
    {
        get
        {
            yield return new object[] { Guid.Empty.ToString(), "true", true };
            yield return new object[] { Guid.Empty.ToString(), "false", false };
            yield return new object[] { Guid.NewGuid().ToString(), "true", false };
        }
    }
}