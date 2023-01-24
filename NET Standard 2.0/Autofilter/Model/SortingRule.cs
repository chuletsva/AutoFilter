namespace Autofilter.Model;

public sealed record SortingRule(string PropertyName, bool IsDescending = false);