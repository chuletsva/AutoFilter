using System.ComponentModel;

namespace Autofilter.Helpers;

static class ValueConverter
{
    private static readonly HashSet<Type> FloatingPointTypes = new()
    {
        typeof(float),
        typeof(float?),
        typeof(double),
        typeof(double?),
        typeof(decimal),
        typeof(decimal?),
    };

    public static object? ConvertValueToType(string? value, Type type)
    {
        if (value is null) return value;

        TypeConverter converter = TypeDescriptor.GetConverter(type);

        if (FloatingPointTypes.Contains(type))
            value = value.Replace(",", ".");

        return converter.ConvertFromInvariantString(value);
    }
}