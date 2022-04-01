namespace Autofilter.Exceptions;

class FilterException : Exception
{
    public FilterException(string? message = default,
        Exception? innerException = default) : base(message, innerException)
    {
    }
}