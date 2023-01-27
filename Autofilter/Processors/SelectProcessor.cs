using System.Dynamic;

namespace Autofilter.Processors;

internal static class SelectProcessor
{
    public static object ApplySelect(IQueryable queryable, string[] propertyNames)
    {
        IQueryable<TestClass> q;

        return queryable;
    }

    private record TestClass(bool Prop);
}