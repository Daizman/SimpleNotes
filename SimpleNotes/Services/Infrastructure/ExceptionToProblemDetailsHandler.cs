using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using SimpleNotes.Errors;

namespace SimpleNotes.Services.Infrastructure;

public class ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemContext = ProblemDetailsContext(context, exception);
        context.Response.StatusCode = (int)problemContext.ProblemDetails.Status!;
        return await problemDetailsService.TryWriteAsync(problemContext);
    }

    private static ProblemDetailsContext ProblemDetailsContext(HttpContext context, Exception exception)
    {
        var (statusCode, msg) = exception switch
        {
            IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
            ValidationException validationException => (StatusCodes.Status400BadRequest, validationException.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unhandled error occurred."),
        };

        ProblemDetailsContext problemContext = new()
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = new()
            {
                Status = statusCode,
                Title = msg,
                Extensions = ValidationExceptionAnswer(exception),
            },
        };
        return problemContext;
    }

    private static Dictionary<string, object?> ValidationExceptionAnswer(Exception? exception)
    {
        Dictionary<string, object?> extension = new();
        if (exception is null)
        {
            return extension;
        }

        if (exception is ValidationException validation)
        {
            foreach (var error in validation.Errors)
            {
                extension[error.PropertyName] = error.ErrorMessage;
            }
        }

        return extension;
    }
}