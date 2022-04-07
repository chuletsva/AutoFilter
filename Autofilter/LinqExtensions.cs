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

public class Ts
{
// queryable.Where(x => ((x.Name.StartsWith("Sni") && x.ExpireDate >= 07.04.2022) || (x.Name.Contains("Mars") && x.IsForSale)) && x.IsInStock)
    public void D()
    {
        IQueryable<Product> queryable = Array.Empty<Product>().AsQueryable();

        Filter filter = new()
        {
            Pagination = new PaginationRule(Skip: 5, Top: 1)
        };

        queryable.ApplyFilter(filter);
    }
}