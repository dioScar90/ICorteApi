using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Exceptions;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = GetTitleForProblemDetails(exception),
            Status = GetStatusCodeForProblemDetails(exception),
            Detail = GetErrorMessageForProblemDetails(exception),
        };

        if (exception is BaseException ex and { Errors.Count: > 0 })
        {
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                ["errors"] = ex.Errors
            };
        }
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }

    private static string GetTitleForProblemDetails(Exception exception) => exception switch
        {
            UnauthorizedException       => "Unauthorized",
            NotFoundException           => "NotFound",
            BadRequestException         => "BadRequest",
            ForbiddenException          => "Forbidden",
            ConflictException           => "Conflict",
            UnprocessableEntity         => "UnprocessableEntity",
            MethodNotAllowedException   => "MethodNotAllowed",
            _ => "ServerError"
        };

    private static int GetStatusCodeForProblemDetails(Exception exception) => exception switch
        {
            UnauthorizedException       => StatusCodes.Status401Unauthorized,
            NotFoundException           => StatusCodes.Status404NotFound,
            BadRequestException         => StatusCodes.Status400BadRequest,
            ForbiddenException          => StatusCodes.Status403Forbidden,
            ConflictException           => StatusCodes.Status409Conflict,
            UnprocessableEntity         => StatusCodes.Status422UnprocessableEntity,
            MethodNotAllowedException   => StatusCodes.Status405MethodNotAllowed,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string? GetErrorMessageForProblemDetails(Exception exception) => exception switch
        {
            UnauthorizedException       => exception.Message ?? null,
            NotFoundException           => exception.Message ?? null,
            BadRequestException         => exception.Message ?? null,
            ForbiddenException          => exception.Message ?? null,
            ConflictException           => exception.Message ?? null,
            UnprocessableEntity         => exception.Message ?? null,
            MethodNotAllowedException   => exception.Message ?? null,
            _ => exception.ToString()
        };
}
