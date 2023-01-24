using System.Text.Json.Serialization;

namespace Autofilter.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchOperator
{
    // All
    Equals = 1,
    NotEquals,

    // Long Int Short Decimal Double Float DateTime Char Byte Nullable
    Greater,
    GreaterOrEqual,
    Less,
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