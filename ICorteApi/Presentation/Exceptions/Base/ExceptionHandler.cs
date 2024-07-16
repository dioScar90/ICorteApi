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
            Detail = exception.Message ?? null
        };
        
        if (exception is BadRequestException br && br.Errors is not null)
        {
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                ["errors"] = br.Errors
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }

    private static string GetTitleForProblemDetails(Exception exception) => exception switch
        {
            NotFoundException   => "NotFound",
            BadRequestException => "BadRequest",
            _ => "ServerError"
        };

    private static int GetStatusCodeForProblemDetails(Exception exception) => exception switch
        {
            NotFoundException   => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
}
