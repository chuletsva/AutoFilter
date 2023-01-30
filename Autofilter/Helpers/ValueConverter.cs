using System.ComponentModel;

namespace Autofilter.Helpers;

internal static class ValueConverter
{
    public static object? ConvertValue(string? value, Type destinationType)
    {
        if (value is null) return null;

        try
        {
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);

            return converter.ConvertFromInvariantString(value);
        }
        catch
        {
            throw new Exception($"Cannot convert value '{value}' to type {destinationType.Name}");
        }
    }

    public static object ConvertArray(string?[] value, Type elementType)
    {
        var arr = Array.CreateInstance(elementType, value.Length);

        for (int i = 0; i < value.Length; i++)
        {
            var condValue = ConvertValue(value[i], elementType);

            arr.SetValue(condValue, i);
        }

        return arr;
    }
}