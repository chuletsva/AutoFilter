using Autofilter.Rules;

namespace Autofilter;

public sealed record AutoFilter(
    FilterRule? Filter = default,
    DistinctRule? Distinct = default,
    SortingRule? Sorting = default,
    int? Skip = default,
    int? Top = default,
    string[]? Select = default);