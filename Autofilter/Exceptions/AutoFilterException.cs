namespace Autofilter.Exceptions;

public sealed class AutoFilterException : Exception
{
    internal AutoFilterException(string message) : base(message)
    {
    }

    internal AutoFilterException(string message, Exception innerException) : base(message, innerException)
    {
    }
}