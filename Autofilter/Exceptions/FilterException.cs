namespace Autofilter.Exceptions;

public sealed class FilterException : Exception
{
    internal FilterException(string message) : base(message)
    {
    }

    internal FilterException(string message, Exception innerException) : base(message, innerException)
    {
    }
}