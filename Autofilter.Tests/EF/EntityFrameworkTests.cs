using Autofilter.Rules;
using Autofilter.Tests.EF.Common;
using Microsoft.EntityFrameworkCore;

namespace Autofilter.Tests.EF;

public class EntityFrameworkTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _db;

    public EntityFrameworkTests(DatabaseFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task WithoutFilter()
    {
        var products = await _db.DbContext.Products.Where(x =>
            ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow) && (x.IsForSale || x.IsInStock))
            .Select(x => new Dictionary<string, object>()
            {
                {nameof(Product.Id), x.Id},
                {nameof(Product.Name), x.Name},
            })
            .Select(x => x[nameof(Product.Id)])
            .ToArrayAsync();
    }

    [Fact]
    public async Task WithFilter()
    {
        AutoFilter filter = CreateFilter();

        var products = await _db.DbContext.Products
            .ApplyFilterDynamic(filter)
            .OfType<object>().ToArrayAsync();
    }

    private static AutoFilter CreateFilter()
    {
        return new AutoFilter
        (
            Filter: new FilterRule
            (
                new[]
                {
                    new Condition
                    (
                        Name: nameof(Product.Name),
                        SearchOperator: SearchOperator.StartsWith,
                        Value: "Snickers"
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.Or,
                        Name: nameof(Product.Name),
                        SearchOperator: SearchOperator.Contains,
                        Value: "Mars"
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.And,
                        Name: nameof(Product.ExpireDate),
                        SearchOperator: SearchOperator.GreaterOrEqual,
                        Value: DateTime.UtcNow.ToString("s")
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.And,
                        Name: nameof(Product.IsForSale),
                        SearchOperator: SearchOperator.Equals,
                        Value: "true"
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.Or,
                        Name: nameof(Product.IsInStock),
                        SearchOperator: SearchOperator.Equals,
                        Value: "true"
                    ),
                },

                new[]
                {
                    new Group
                    (
                        Start: 1,
                        End: 2,
                        Level: 1
                    ),
                    new Group
                    (
                        Start: 1,
                        End: 3,
                        Level: 2
                    ),
                    new Group
                    (
                        Start: 4,
                        End: 5,
                        Level: 2
                    )
                }
            ),
            Select: new []
            {
                nameof(Product.Id),
                nameof(Product.Name)
            }
        );
    }
}