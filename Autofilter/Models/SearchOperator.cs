namespace Autofilter.Models;

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