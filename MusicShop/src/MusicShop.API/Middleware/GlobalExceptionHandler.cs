using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace MusicShop.API.Middleware;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IWebHostEnvironment webHostEnvironment)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        ProblemDetails problemDetails = new();

        if (exception is ValidationException validationException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Validation Error";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(validationError => validationError.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(validationError => validationError.ErrorMessage).ToArray()
                );
        }
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Internal Server Error";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            
            bool isDev = webHostEnvironment.IsDevelopment();
            problemDetails.Detail = isDev ? exception.ToString() : null;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
