using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Presentation.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (!IsMappedExceptionType(exception))
            return false;
        
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        
        var problemDetails = new ProblemDetails
        {
            Title = GetTitleForProblemDetails(exception),
            Status = GetStatusCodeForProblemDetails(exception),
            Detail = GetErrorMessageForProblemDetails(exception),
        };

        SetSpecificProblemDetailsIfItMust(exception, problemDetails);
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
    
    private static void SetSpecificProblemDetailsIfItMust(Exception exception, ProblemDetails problemDetails) 
    {
        static Dictionary<string, object?> StartNewDictionary() => [];
        
        if (exception is BaseException bEx)
        {
            problemDetails.Extensions ??= StartNewDictionary();
            problemDetails.Extensions["errors"] = bEx.Errors;
        }

        if (exception is DbUpdateException dbEx)
        {
            problemDetails.Extensions ??= StartNewDictionary();
            problemDetails.Extensions["dbProblem"] = dbEx.InnerException?.Message ?? "Unknown problem";
            problemDetails.Extensions["dbErrors"] = dbEx.Entries;
        }

        if (exception is ValidationException vEx)
        {
            problemDetails.Extensions ??= StartNewDictionary();
            problemDetails.Extensions["validationProblem"] = vEx.InnerException?.Message ?? "Unknown problem";
            problemDetails.Extensions["validationErrors"] = vEx.Errors;
        }
    }

    private static bool IsMappedExceptionType(Exception exception) =>
        exception
            is UnauthorizedException
            or NotFoundException
            or BadRequestException
            or ForbiddenException
            or ConflictException
            or UnprocessableEntity
            or MethodNotAllowedException
            or BadHttpRequestException
            or ArgumentNullException
            or ArgumentException
            or UnauthorizedAccessException
            or InvalidOperationException
            or KeyNotFoundException
            or NotSupportedException
            or TimeoutException
            or DbUpdateException
            or ValidationException
        ;

    private static string GetTitleForProblemDetails(Exception exception) => exception switch
        {
            // Custom exceptions
            UnauthorizedException       => "Unauthorized",
            NotFoundException           => "NotFound",
            BadRequestException         => "BadRequest",
            ForbiddenException          => "Forbidden",
            ConflictException           => "Conflict",
            UnprocessableEntity         => "UnprocessableEntity",
            MethodNotAllowedException   => "MethodNotAllowed",

            // Native exceptions
            BadHttpRequestException     => "BadHttpRequest",
            ArgumentNullException       => "ArgumentNull",
            ArgumentException           => "Argument",
            UnauthorizedAccessException => "UnauthorizedAccess",
            InvalidOperationException   => "InvalidOperation",
            KeyNotFoundException        => "KeyNotFound",
            NotSupportedException       => "NotSupported",
            TimeoutException            => "Timeout",

            // Entity Framework Excceptions
            DbUpdateException           => "DbUpdate",

            // Fluent Validation Excceptions
            ValidationException         => "ValidationException",

            _ => "ServerError"
        };

    private static int GetStatusCodeForProblemDetails(Exception exception) => exception switch
    {
        // Custom exceptions
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        NotFoundException => StatusCodes.Status404NotFound,
        BadRequestException => StatusCodes.Status400BadRequest,
        ForbiddenException => StatusCodes.Status403Forbidden,
        ConflictException => StatusCodes.Status409Conflict,
        UnprocessableEntity => StatusCodes.Status422UnprocessableEntity,
        MethodNotAllowedException => StatusCodes.Status405MethodNotAllowed,

        // Native exceptions
        BadHttpRequestException => StatusCodes.Status400BadRequest,
        ArgumentNullException => StatusCodes.Status400BadRequest,
        ArgumentException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
        InvalidOperationException => StatusCodes.Status500InternalServerError,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        NotSupportedException => StatusCodes.Status405MethodNotAllowed,
        TimeoutException => StatusCodes.Status504GatewayTimeout,

        // Entity Framework Excceptions
        DbUpdateException => StatusCodes.Status500InternalServerError,

        // Fluent Validation Excceptions
        ValidationException => StatusCodes.Status409Conflict,

        _ => StatusCodes.Status500InternalServerError
    };

    private static string? GetErrorMessageForProblemDetails(Exception exception) => exception switch
    {
        // Custom exceptions
        UnauthorizedException ex => ex?.Message ?? null,
        NotFoundException ex => ex?.Message ?? null,
        BadRequestException ex => ex?.Message ?? null,
        ForbiddenException ex => ex?.Message ?? null,
        ConflictException ex => ex?.Message ?? null,
        UnprocessableEntity ex => ex?.Message ?? null,
        MethodNotAllowedException ex => ex?.Message ?? null,

        // Native exceptions
        BadHttpRequestException ex => ex?.Message ?? null,
        ArgumentNullException ex => ex?.Message ?? null,
        ArgumentException ex => ex?.Message ?? null,
        UnauthorizedAccessException ex => ex?.Message ?? null,
        InvalidOperationException ex => ex?.Message ?? null,
        KeyNotFoundException ex => ex?.Message ?? null,
        NotSupportedException ex => ex?.Message ?? null,
        TimeoutException ex => ex?.Message ?? null,

        // EF Excceptions
        DbUpdateException ex => ex?.Message ?? null,

        _ => exception?.Message ?? "Erro desconhecido"
    };
}
