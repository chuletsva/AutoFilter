namespace Autofilter.Model;

public sealed record SearchRule(
    string PropertyName, string? Value, 
    SearchOperator SearchOperator,
    LogicOperator? LogicOperator = default);