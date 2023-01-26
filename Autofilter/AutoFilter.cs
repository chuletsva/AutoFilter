using Autofilter.Rules;

namespace Autofilter;

public sealed record AutoFilter(
    FilterRule? Filter = default,
    string? DistinctBy = default,
    SortingRule? Sorting = default,
    int? Skip = default,
    int? Top = default,
    string[]? Select = default);