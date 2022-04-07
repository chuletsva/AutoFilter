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

## Examples

Using parentheses

```c#
queryable.Where(x => ((x.Name.StartsWith("Sni") && x.ExpireDate >= "07.04.2022") || (x.Name.Contains("Mars") && x.IsForSale)) && x.IsInStock)
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
```

```c#
```