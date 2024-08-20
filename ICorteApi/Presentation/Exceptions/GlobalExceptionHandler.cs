using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
            or TimeoutException;

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

        _ => exception?.Message ?? "Erro desconhecido"
    };
}


/*
Aqui está a lista das exceptions mais comuns em ASP.NET com seus respectivos status codes recomendados:

BadHttpRequestException
Descrição: Disparada quando há um problema com a solicitação HTTP, como falha ao analisar o corpo da solicitação (por exemplo, JSON malformado ou tipo de conteúdo inválido).
Status Code: 400 Bad Request

ArgumentNullException
Descrição: Disparada quando um argumento obrigatório de um método é null.
Status Code: 400 Bad Request

ArgumentException
Descrição: Disparada quando um argumento inválido é passado para um método (exemplo: string vazia ou valor fora do intervalo esperado).
Status Code: 400 Bad Request

UnauthorizedAccessException
Descrição: Disparada quando uma operação tenta acessar um recurso para o qual o usuário não tem permissão.
Status Code: 401 Unauthorized

InvalidOperationException
Descrição: Disparada quando uma operação não é válida devido ao estado atual do objeto (exemplo: tentar realizar uma operação em um objeto mal configurado).
Status Code: 500 Internal Server Error (pode ser 409 Conflict se a operação falhar devido a um estado inconsistente que pode ser resolvido pelo cliente)

KeyNotFoundException
Descrição: Disparada quando uma chave não é encontrada em um dicionário ou coleção semelhante.
Status Code: 404 Not Found

NotSupportedException
Descrição: Disparada quando um método invocado não é suportado (exemplo: uma operação não implementada ou não aplicável).
Status Code: 405 Method Not Allowed ou 501 Not Implemented

TimeoutException
Descrição: Disparada quando uma operação assíncrona, como uma chamada de banco de dados ou uma solicitação HTTP, excede o tempo limite especificado.
Status Code: 504 Gateway Timeout

Esses códigos de status podem ser usados
*/