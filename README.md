Create a model

```c#
class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int InStock { get; set; }
    public bool IsForSale { get; set; }
}
```

Build a filter

```c#
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
```

Apply filter to queryable

```c#
queryable.ApplyFilter(filter);
```

Under the hood our filter transforms into call
```c#
queryable.Where(x => x.Name.StartsWith("Sni") && x.InStock);
```