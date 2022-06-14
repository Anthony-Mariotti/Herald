namespace Herald.Core.Exceptions;

public class LavalinkException : Exception
{
    public LavalinkException(string? message)
        : base(message)
    {}
    
    public LavalinkException(string? message, Exception? innerException)
        : base(message, innerException)
    {}
}