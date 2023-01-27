namespace Autofilter.Rules;

public sealed record FilterRule(Condition[] Conditions, Group[]? Groups = default);

public sealed record Condition(string Name, string? Value, SearchOperator SearchOperator, LogicOperator? LogicOperator = default);

public sealed record Group(int Start, int End, int Level);

public enum LogicOperator { And = 1, Or }

public enum SearchOperator
{
    // All
    Equals = 1,
    NotEquals,

    // Long, Int, Short, Decimal, Double, Float, DateTime, Char, Byte, Nullable
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual,

    // Nullable, String
    Exists,
    NotExists,

    // String
    StartsWith,
    EndsWith,
    Contains,
    NotContains
}
