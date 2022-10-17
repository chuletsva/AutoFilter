[![Nuget](https://img.shields.io/nuget/v/ART4S.Autofilter)](https://www.nuget.org/packages/ART4S.Autofilter/)
# Autofilter

Autofilter simplifies data filtering using Expression Trees. 
Reduces amount of code needed for manual writing filter logic on backend side, enabling this work to be done on frontend

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
            SearchOperator: SearchOperator.Contains,
            Value: "Snickers",
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.And,
            PropertyName: "IsForSale",
            SearchOperator: SearchOperator.Equals,
            Value: "true"
        ),
    }
};
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
            LogicOperator: LogicOperator.Or,
            PropertyName: "Name",
            SearchOperator: SearchOperator.Contains,
            Value: "Mars",
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.And,
            PropertyName: "ExpireDate",
            SearchOperator: SearchOperator.GreaterOrEqual,
            Value: "07.04.2022"
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.And,
            PropertyName: "IsForSale",
            SearchOperator: SearchOperator.Equals,
            Value: "true"
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

#### Sorting

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

#### Paging

```c#
queryable.Skip(5).Take(1);
```

```c#
Filter filter = new()
{
    Pagination = new PaginationRule(Skip: 5, Top: 1)
};
```

## Operations vs Types compatibility

![Compatibility1](https://user-images.githubusercontent.com/24371700/162436461-09717eaa-23d4-4693-af71-eed40aab02ee.png) 
![Compatibility2](https://user-images.githubusercontent.com/24371700/162436470-3e3db5e0-ab62-4add-bdb1-91664017a4e6.png)
![Compatibility3](https://user-images.githubusercontent.com/24371700/162436496-2d995028-8e68-48f1-8c67-5698792a5527.png)
