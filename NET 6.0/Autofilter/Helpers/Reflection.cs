using System.Collections.Concurrent;
using System.Reflection;

namespace Autofilter.Helpers;

internal static class Reflection
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, PropertyInfo>> PropertiesCache = new();

    private static readonly HashSet<Type> ComparableTypes = new()
    {
        typeof(long), typeof(int), typeof(short),
        typeof(decimal), typeof(double), typeof(float),
        typeof(byte), typeof(char), typeof(DateTime)
    };

    public static bool IsComparable(Type type)
    {
        return ComparableTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
    }

    public static bool CanBeNull(Type type)
    {
        return type.IsClass || Nullable.GetUnderlyingType(type) is not null;
    }

    public static PropertyInfo GetProperty(Type type, string propertyName)
    {
        var properties = PropertiesCache.GetOrAdd(type, t => t.GetProperties().ToDictionary(p => p.Name));

        if (!properties.TryGetValue(propertyName, out var property))
        {
            throw new Exception($"Property '{propertyName}' for type '{type.Name}' doesn't exist");
        }

        return property;
    }
}