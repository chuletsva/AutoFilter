using System.Runtime.CompilerServices;
using Autofilter.Model;
using Autofilter.Processors;

[assembly: InternalsVisibleTo("Autofilter.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Autofilter;

public static class LinqExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, Filter filter)
    {
        return FilterProcessor.Instance.ApplyFilter(source, filter);
    }
}

class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int InStock { get; set; }
    public bool IsForSale { get; set; }
    public DateTime ExpireDate { get; set; }
}

public class T
{
// queryable.Where(x => ((x.Name.StartsWith("Sni") && x.ExpireDate >= 07.04.2022) || (x.Name.Contains("Mars") && x.IsForSale)) && x.IsInStock)
    public void D()
    {
        IQueryable<Product> queryable = Array.Empty<Product>().AsQueryable();

        Filter filter = new()
        {
            Search = new[]
            {
                new SearchRule
                (
                    PropertyName: "Name",
                    SearchOperator: SearchOperator.StartsWith,
                    Value: "Sni"
                ),
                new SearchRule
                (
                    PropertyName: "ExpireDate",
                    SearchOperator: SearchOperator.GreaterOrEqual,
                    Value: "07.04.2022",
                    LogicOperator: LogicOperator.And
                ),
                new SearchRule
                (
                    PropertyName: "InStock",
                    SearchOperator: SearchOperator.Contains,
                    Value: "Mars",
                    LogicOperator: LogicOperator.Or
                ),
                new SearchRule
                (
                    PropertyName: "IsForSale",
                    SearchOperator: SearchOperator.Equals,
                    Value: "true",
                    LogicOperator: LogicOperator.And
                ),
                new SearchRule
                (
                    PropertyName: "IsInStock",
                    SearchOperator: SearchOperator.Equals,
                    Value: "true",
                    LogicOperator: LogicOperator.And
                ),
            },
            Groups = new[]
            {
                new GroupRule
                (
                    Start: 0,
                    End: 1,
                    Level: 1
                ),
                new GroupRule
                (
                    Start: 2,
                    End: 3,
                    Level: 1
                ),
                new GroupRule
                (
                    Start: 0,
                    End: 3,
                    Level: 2
                ),
            }
        };

        queryable.ApplyFilter(filter);
    }
}