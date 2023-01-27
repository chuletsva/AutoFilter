using Autofilter.Rules;

namespace Autofilter;

public sealed record AutoFilter(
    FilterRule? Filter = default,
    SortingRule? Sorting = default,
    DistinctRule? Distinct = default,
    string[]? Select = default,
    int? Skip = default,
    int? Top = default);