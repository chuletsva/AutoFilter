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
}

public class T
{
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
                    Value: "Sni",
                    SearchOperator: SearchOperator.StartsWith
                ),
                new SearchRule
                (
                    PropertyName: "InStock",
                    Value: "true",
                    SearchOperator: SearchOperator.Equals,
                    LogicOperator: LogicOperator.And
                ),
            }
        };

        queryable.ApplyFilter(filter);
    }
}