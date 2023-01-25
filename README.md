![Nuget](https://img.shields.io/nuget/v/ART4S.Autofilter?label=net%20standard)
# Autofilter

Autofilter simplifies data filtering using Expression Trees. 
Reduces amount of code needed for manual writing filter logic on backend side, enabling this work to be done on frontend

Inspired by Max Arshinov's [article](https://habr.com/ru/company/jugru/blog/423891/).

Any issues or criticism are welcome :)

## Introduction

Create a model

```c#
class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
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
            Name: "Name",
            SearchOperator: SearchOperator.Contains,
            Value: "Snickers",
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.And,
            Name: "IsForSale",
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
```c#
queryable.Where(x => ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= "07.04.2022") && (x.IsForSale || x.IsInStock))
```
```c#
Filter filter = new()
{
    Search = new[]
    {
        new SearchRule
        (
            Name: "Name",
            SearchOperator: SearchOperator.StartsWith,
            Value: "Snickers"
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.Or,
            Name: "Name",
            SearchOperator: SearchOperator.Contains,
            Value: "Mars",
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.And,
            Name: "ExpireDate",
            SearchOperator: SearchOperator.GreaterOrEqual,
            Value: "07.04.2022"
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.And,
            Name: "IsForSale",
            SearchOperator: SearchOperator.Equals,
            Value: "true"
        ),
        new SearchRule
        (
            LogicOperator: LogicOperator.Or,
            Name: "IsInStock",
            SearchOperator: SearchOperator.Equals,
            Value: "true"
        ),
    },
    Groups = new[]
    {
        new GroupRule
        (
            Start: 1,
            End: 2,
            Level: 1
        ),
        new GroupRule
        (
            Start: 1,
            End: 3,
            Level: 2
        ),
        new GroupRule
        (
            Start: 4,
            End: 5,
            Level: 2
        )
    }
};
```

#### Sorting

```c#
queryable.OrderBy(x => x.ExpireDate).ThenByDescending(x => x.Price)
```
```c#
Filter filter = new()
{
    Sorting = new []
    {
        new SortingRule("ExpireDate"),
        new SortingRule("Price", true)
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
    Pagination = new PaginationRule(5, 1)
};
```

## Operations vs Types compatibility

![Compatibility1](https://user-images.githubusercontent.com/24371700/162436461-09717eaa-23d4-4693-af71-eed40aab02ee.png) 
![Compatibility2](https://user-images.githubusercontent.com/24371700/162436470-3e3db5e0-ab62-4add-bdb1-91664017a4e6.png)
![Compatibility3](https://user-images.githubusercontent.com/24371700/162436496-2d995028-8e68-48f1-8c67-5698792a5527.png)
