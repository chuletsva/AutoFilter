# Autofilter

Autofilter simplifies data filtering using Expression Trees. 
Reduces the need for writing filtering logic on backend side.

Inspired by Max Arshinov's [article](https://habr.com/ru/company/jugru/blog/423891/).

All operations were tested against EF Core and PostgreSql.

Any issues or criticism are welcome :)

## Introduction

Create a model

```c#
class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
    public bool IsForSale { get; set; }
    public DateTime ExpireDate { get; set; }
}
```

Build a filter

```c#
var filter = new AutoFilter
(
    Filter: new FilterRule
    (
        Conditions: new[]
        {
            new Condition
            (
                Name: "Name",
                SearchOperator: SearchOperator.Contains,
                Value: new[]{ "Snickers" }
            ),
            new Condition
            (
                LogicOperator: LogicOperator.And,
                Name: "IsForSale",
                SearchOperator: SearchOperator.Equals,
                Value: new[] { "true" }
            ),
        }
    )
);
```

Apply filter to queryable

```c#
queryable.ApplyFilter(filter);
```

Under the hood filter transforms into call
```c#
queryable.Where(x => x.Name.Contains("Snickers") && x.IsForSale);
```

## Examples

#### Filtering
```c#
queryable.Where(x => ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow) && (x.IsForSale || x.IsInStock))
```
```c#
var filter = new AutoFilter
(
    Filter: new FilterRule
    (
        new Condition[]
        {
            new
            (
                Name: nameof(Product.Name),
                SearchOperator: SearchOperator.StartsWith,
                Value: new[]{ "Snickers" }
            ),
            new
            (
                LogicOperator: LogicOperator.Or,
                Name: nameof(Product.Name),
                SearchOperator: SearchOperator.Contains,
                Value: new[]{ "Mars" }
            ),
            new
            (
                LogicOperator: LogicOperator.And,
                Name: nameof(Product.ExpireDate),
                SearchOperator: SearchOperator.GreaterOrEqual,
                Value: new[] { DateTime.UtcNow.ToString("u") }
            ),
            new
            (
                LogicOperator: LogicOperator.And,
                Name: nameof(Product.IsForSale),
                SearchOperator: SearchOperator.Equals,
                Value: new[] { "true" }
            ),
            new
            (
                LogicOperator: LogicOperator.Or,
                Name: nameof(Product.IsInStock),
                SearchOperator: SearchOperator.Equals,
                Value: new[] { "true" }
            ),
        },

        new Group[]
        {
            new
            (
                Start: 1,
                End: 2,
                Level: 1
            ),
            new
            (
                Start: 1,
                End: 3,
                Level: 2
            ),
            new
            (
                Start: 4,
                End: 5,
                Level: 2
            )
        }
    )
);

queryable.ApplyFilter(filter);
```

#### Distinct
```c#
queryable.Distinct();
```
```c#
var filter = new AutoFilter
(
    DistinctBy: ""
);

queryable.ApplyFilter(filter);
```



```c#
queryable.DistinctBy(x => x.Name);
```

```c#
var filter = new AutoFilter
(
    DistinctBy: "Name"
);

queryable.ApplyFilter(filter);
```

#### Sorting

```c#
queryable.OrderBy(x => x.ExpireDate);
```
```c#
var filter = new AutoFilter
(
    Sorting: new SortingRule
    (
        PropertyName: "ExpireDate"
    )
);

queryable.ApplyFilter(filter);
```

```c#
queryable.OrderBy(x => x.ExpireDate).ThenByDescending(x => x.Price);
```
```c#
var filters = new AutoFilter[]
{
    new
    (
        Sorting: new SortingRule
        (
            PropertyName: "ExpireDate"
        )
    ),
    new
    (
        Sorting: new SortingRule
        (
            PropertyName: "Price",
            ThenBy: true,
            IsDescending: true
        )
    ),
};

queryable.ApplyFilters(filters);
```

#### Paging

```c#
queryable.Skip(5).Take(1);
```

```c#
var filters = new[]
{
    new AutoFilter(Skip: 5),
    new AutoFilter(Top: 1)
};

queryable.ApplyFilters(filters);
```

#### Select
```c#
queryable.Select(x => new Dictionary<string, object>()
{
    { "Id", x.Id },
    { "Name", x.Name }
})
```
```c#   
var filter = new AutoFilter
(
    Select: new[] { "Id", "Name" }
);

queryable.ApplyFilterAndSelect(filter);
```

This implementation of "Select" was made due to the limitations of anonymous types and should be used as the last operation 
and only to reduce the final number of properties to be fetched from the database.

## Operations vs Types compatibility

![Compatibility1](https://user-images.githubusercontent.com/24371700/162436461-09717eaa-23d4-4693-af71-eed40aab02ee.png) 
![Compatibility2](https://user-images.githubusercontent.com/24371700/162436470-3e3db5e0-ab62-4add-bdb1-91664017a4e6.png)
![Compatibility3](https://user-images.githubusercontent.com/24371700/162436496-2d995028-8e68-48f1-8c67-5698792a5527.png)
