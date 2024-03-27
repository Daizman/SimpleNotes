using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using SimpleNotes.Errors;

namespace SimpleNotes.Endpoints;

public static class ErrorEndpoints
{
    public static void MapErrorEndpoints(this IEndpointRouteBuilder app)
    {
        app.Map("/error", async (HttpContext context) =>
        {
            var pds = context.RequestServices.GetRequiredService<IProblemDetailsService>();
            var problemContext = ProblemDetailsContext(context);

            if (!await pds.TryWriteAsync(problemContext))
            {
                await context.Response.WriteAsync("Fallback: An error occurred.");
            }

            // toDo: разобраться с 500
            return Results.Problem(problemContext.ProblemDetails);
        });
    }

    private static ProblemDetailsContext ProblemDetailsContext(HttpContext context)
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
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