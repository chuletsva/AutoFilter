using System.ComponentModel;

namespace Autofilter.Helpers;

internal static class ValueConverter
{
    public static object? ConvertValueToType(string? value, Type type)
    {
        if (value is null) return null;

        try
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);

            return converter.ConvertFromInvariantString(value);
        }
        catch
        {
            throw new Exception($"Cannot convert {GetInvalidValueAlias(value)} to type '{type.Name}'");
        }
    }

    public static string GetInvalidValueAlias(object? value)
    {
        return value switch
        {
            null => "null",
            _ when string.IsNullOrWhiteSpace(value.ToString()) => "empty string",
            _ => $"value '{value}'"
        };
    }
}