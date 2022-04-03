namespace Autofilter.Model;

public sealed record PaginationRule(int? Skip, int? Top)
{
    public void Deconstruct(out int? skip, out int? top)
        => (skip, top) = (Skip, Top);
}