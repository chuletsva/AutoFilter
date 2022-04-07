namespace Autofilter.Exceptions;

class FilterException : Exception
{
    public FilterException(string message, Exception innerException) : base(message, innerException)
    {
    }
}