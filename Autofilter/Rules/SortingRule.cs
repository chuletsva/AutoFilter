namespace Autofilter.Rules;

public sealed record SortingRule(string PropertyName, bool ThenBy = false, bool IsDescending = false);