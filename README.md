# Autofilter

Autofilter simplify data filtering using well known technology Expression Trees. 
Reduces amount of code needed for manually writing filtering logic on backend side, allowing this work to be done on frontend

Inspired by Max Arshinov's [article](https://habr.com/ru/company/jugru/blog/423891/) 

## Introduction

Create a model

```c#
class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int InStock { get; set; }
    public bool IsForSale { get; set; }
    public DateTime ExpireDate { get; set; }
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
            SearchOperator: SearchOperator.StartsWith,
            Value: "Sni",
        ),
        new SearchRule
        (
            PropertyName: "IsForSale",
            SearchOperator: SearchOperator.Equals,
            Value: "true",
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
queryable.Where(x => x.Name.StartsWith("Sni") && x.IsForSale);
```

## Examples

Using parentheses

```c#
queryable.Where(x => ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= "07.04.2022") && x.IsForSale)
```
```c#
Filter filter = new()
{
    Search = new[]
    {
        new SearchRule
        (
            PropertyName: "Name",
            SearchOperator: SearchOperator.StartsWith,
            Value: "Snickers"
        ),
        new SearchRule
        (
            PropertyName: "Name",
            SearchOperator: SearchOperator.Contains,
            Value: "Mars",
            LogicOperator: LogicOperator.Or
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
            PropertyName: "IsForSale",
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
            Start: 0,
            End: 2,
            Level: 2
        )
    }
};
```

Sorting

```c#
queryable.OrderBy(x => x.ExpireDate).ThenByDescending(x => x.InStock)
```
```c#
Filter filter = new()
{
    Sorting = new []
    {
        new SortingRule(PropertyName: "ExpireDate"),
        new SortingRule(PropertyName: "InStock", IsDescending: true)
    }
};
```

Paging

```c#
queryable.Skip(5).Take(1);
```

```c#
Filter filter = new()
{
    Pagination = new PaginationRule(Skip: 5, Top: 1)
};
```