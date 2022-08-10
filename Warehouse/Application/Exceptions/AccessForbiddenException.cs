using Application.Infrastructure.Context;

namespace Application.Exceptions;

public class AccessForbiddenException : Exception
{
    public AccessForbiddenException() : base()
    {
    }

    public AccessForbiddenException(string message) : base(message)
    {
        
    }
}