namespace Autofilter.Processors;

internal static class PaginationProcessor
{
    public static IQueryable ApplySkip(IQueryable queryable, int skip)
    {
        var method = typeof(Queryable).GetMethods()
            .Single(x => x.Name == "Skip" && x.GetParameters().Length == 2)
            .MakeGenericMethod(queryable.ElementType);

        var skipQueryable = method.Invoke(null, new object[] { queryable, skip }) ?? throw new NullReferenceException();

        return (IQueryable) skipQueryable;
    }

    public static IQueryable ApplyTop(IQueryable queryable, int top)
    {
        var method = typeof(Queryable).GetMethods()
            .First(x => x.Name == "Take" && x.GetParameters().Length == 2)
            .MakeGenericMethod(queryable.ElementType);

        var skipQueryable = method.Invoke(null, new object[] { queryable, top }) ?? throw new NullReferenceException();

        return (IQueryable) skipQueryable;
    }
}
