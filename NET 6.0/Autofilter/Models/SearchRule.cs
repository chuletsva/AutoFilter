namespace Autofilter.Models;

public sealed record SearchRule(
    string Name, string? Value, 
    SearchOperator SearchOperator,
    LogicOperator? LogicOperator = default);