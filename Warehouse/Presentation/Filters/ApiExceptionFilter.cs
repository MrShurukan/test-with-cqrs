using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilter()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            [typeof(ValidationException)] = ValidationExceptionHandler,
            [typeof(NotFoundException)] = NotFoundExceptionHandler,
            [typeof(AccessForbiddenException)] = AccessForbiddenException
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (context.ModelState.IsValid) return;
        
        HandleInvalidModelStateException(context);
    }
    
    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private static void ValidationExceptionHandler(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private static void NotFoundExceptionHandler(ExceptionContext context)
    {
        throw new NotImplementedException();
    }

    private static void AccessForbiddenException(ExceptionContext context)
    {
        throw new NotImplementedException();
    }
}