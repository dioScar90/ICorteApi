using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Presentation.Exceptions;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ICustomException)
        {
            // if (exception is ValidationException validationException)
            //     _logger.LogWarning(validationException, "Validation Exception occurred: {Message}", validationException.Message);
            // else
                _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        }
        
        httpContext.Response.StatusCode = GetStatusCodeForProblemDetails(exception);

        return await problemDetailsService.TryWriteAsync(new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new()
            {
                Type = exception.GetType().Name,
                Title = GetTitleForProblemDetails(exception),
                Status = httpContext.Response.StatusCode,
                Detail = exception.Message,
                Extensions = GetProblemDetailsExtensions(exception)
            }
        });
    }

    private static Dictionary<string, object?> GetProblemDetailsExtensions(Exception exception)
    {
        Dictionary<string, object?> extensions = [];
        
        void setProps(Dictionary<string, object> keys)
        {
            foreach (var (key, value) in keys)
                extensions[key] = value;
        }

        string getUnknownMessage(Exception _exception) => $"Unknown {_exception.GetType().Name} problem";

        if (exception is ICustomException bEx)
            setProps(new()
            {
                ["errors"] = bEx.Errors
            });

        if (exception is DbUpdateException dbEx)
            setProps(new()
            {
                ["dbProblem"] = dbEx.InnerException?.Message ?? getUnknownMessage(dbEx),
                ["dbErrors"] = dbEx.Entries
            });

        if (exception is SqlException sEx)
            setProps(new()
            {
                ["dbProblem"] = sEx.InnerException?.Message ?? getUnknownMessage(sEx),
                ["dbErrors"] = sEx.Errors
            });

        // if (exception is ValidationException vEx)
        //     setProps(new()
        //     {
        //         ["validationProblem"] = vEx.InnerException?.Message ?? getUnknownMessage(vEx),
        //         ["validationErrors"] = vEx.Errors
        //     });

        return extensions;
    }
    
    private static string GetTitleForProblemDetails(Exception exception) => exception switch
    {
        ICustomException mappedException => mappedException.Title,
        _ => "Server Error"
    };
    
    private static int GetStatusCodeForProblemDetails(Exception exception) => exception switch
    {
        ICustomException mappedException => mappedException.HttpStatusCode,

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
        // ValidationException => StatusCodes.Status409Conflict,

        _ => StatusCodes.Status500InternalServerError
    };
}
