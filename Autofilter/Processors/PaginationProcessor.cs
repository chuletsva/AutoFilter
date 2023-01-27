using Autofilter.Helpers;

namespace Autofilter.Processors;

internal static class PaginationProcessor
{
    public static IQueryable ApplySkip(IQueryable queryable, int skip)
    {
        var method = LinqMethods.Skip(queryable.ElementType);

        var skipQueryable = method.Invoke(null, new object[] { queryable, skip }) ?? throw new NullReferenceException();

        return (IQueryable) skipQueryable;
    }

    public static IQueryable ApplyTop(IQueryable queryable, int top)
    {
        var method = LinqMethods.Take(queryable.ElementType);

        var topQueryable = method.Invoke(null, new object[] { queryable, top }) ?? throw new NullReferenceException();

        return (IQueryable) topQueryable;
    }
}
