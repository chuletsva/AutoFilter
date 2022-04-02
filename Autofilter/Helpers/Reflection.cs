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

    public static bool IsComparableType(Type type)
        => ComparableTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);

    public static bool IsNullableType(Type type)
        => Nullable.GetUnderlyingType(type) is not null;

    public static bool TryGetProperty<T>(string propertyName, out PropertyInfo property)
        => TypeInfo<T>.Properties.TryGetValue(propertyName, out property);

    private static class TypeInfo<T>
    {
        public static readonly IReadOnlyDictionary<string, PropertyInfo> Properties =
            typeof(T).GetProperties().ToDictionary(x => x.Name);
    }
}