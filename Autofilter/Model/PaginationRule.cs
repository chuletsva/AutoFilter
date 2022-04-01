using Autofilter.Exceptions;

namespace Autofilter.Model;

public sealed record PaginationRule
{
    public PaginationRule(int? skip, int? top)
    {
        if (skip is < 1) throw new FilterException("'Skip' shouldn't be less than 1");
        if (top is < 1) throw new FilterException("'Top' shouldn't be less than 1");

        (Skip, Top) = (skip, top);
    }

    public int? Skip { get; }
    public int? Top { get; }

    public void Deconstruct(out int? skip, out int? top)
        => (skip, top) = (Skip, Top);
}