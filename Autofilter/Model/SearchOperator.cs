using System.Linq.Expressions;

namespace Autofilter.Model;

// Guid
// Long
// Int
// Short
// Decimal
// Double
// Float
// DateTime
// Char
// Bool
// Byte
// Enum
// String

// Nullable

public enum SearchOperator
{
    /* All */

    Equals = 1,
    //NotEquals,

    /* Long Int Short Decimal Double Float DateTime Char Byte */

    Greater,
    //Less,
    //GreaterOrEqual,
    //LessOrEqual,

    /* Nullable String */

    Exists,
    NotExists,

    /* String */

    StartsWith,
    EndsWith,
    Contains,
    NotContains
}

//class C
//{
//    private string? e = null;
//    private  string? e1 = null;

//    void E()
//    {
//        Expression<Func<C, bool>> a = x => e <= e1;
//    }
//}