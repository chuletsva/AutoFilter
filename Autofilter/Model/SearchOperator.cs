namespace Autofilter.Model;

public enum SearchOperator
{
    // All
    Equals = 1,
    NotEquals,

    // Long Int Short Decimal Double Float DateTime Char Byte
    Greater,
    Less,
    GreaterOrEqual,
    LessOrEqual,

    // Nullable String
    Exists,
    NotExists,

    // String
    StartsWith,
    EndsWith,
    Contains,
    NotContains
}