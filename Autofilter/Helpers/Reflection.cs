using System.Collections.Concurrent;
using System.Reflection;

namespace Autofilter.Helpers;

static class Reflection
{
    private static readonly HashSet<Type> ComparableTypes = new()
    {
        typeof(long), typeof(int), typeof(short),
        typeof(decimal), typeof(double), typeof(float),
        typeof(byte), typeof(char), typeof(DateTime)
    };

    private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, PropertyInfo>> Properties = new();

    public static bool IsComparableType(Type type)
        => ComparableTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);

    public static bool IsNullableType(Type type)
        => type.IsClass || Nullable.GetUnderlyingType(type) is not null;

    public static bool TryGetProperty(Type type, string propertyName, out PropertyInfo property)
    {
        return Properties.GetOrAdd(type, t => t.GetProperties().ToDictionary(p => p.Name))
            .TryGetValue(propertyName, out property);
    }
}