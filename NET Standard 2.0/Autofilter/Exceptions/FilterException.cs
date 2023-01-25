namespace Autofilter.Exceptions;

public class FilterException : Exception
{
    internal FilterException(string message, Exception innerException) : base(message, innerException)
    {
    }
}