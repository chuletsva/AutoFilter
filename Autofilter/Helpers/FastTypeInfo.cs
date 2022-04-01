using System.Reflection;

namespace Autofilter.Helpers;

static class FastTypeInfo<T>
{
    private static readonly IReadOnlyDictionary<string, PropertyInfo> Properties =
        typeof(T).GetProperties().ToDictionary(x => x.Name);

    public static bool TryGetProperty(string propertyName, out PropertyInfo property)
    {
        return Properties.TryGetValue(propertyName, out property);
    }
}