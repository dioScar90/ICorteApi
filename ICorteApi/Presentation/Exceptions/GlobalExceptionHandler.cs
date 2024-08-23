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

        if (IsAnExceptionForProblemDetails(exception))
        {
            problemDetails.Extensions ??= new Dictionary<string, object?>();

            if (exception is BaseException bEx)
                problemDetails.Extensions["errors"] = bEx.Errors;

            if (exception is DbUpdateException dbEx)
                problemDetails.Extensions["realProblem"] = dbEx.InnerException?.Message ?? "Unknown problem";
        }
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static bool IsAnExceptionForProblemDetails(Exception exception) =>
        exception
            is BaseException
            or DbUpdateException
        ;

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

            // EF Excceptions
            DbUpdateException           => "DbUpdate",

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

        // EF Excceptions
        DbUpdateException => StatusCodes.Status500InternalServerError,

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





/*
No Entity Framework, as exceptions mais comuns que você pode encontrar são:

DbUpdateException: Ocorre quando há um erro ao salvar alterações no banco de dados. Pode envolver problemas de relacionamento, violação de chave estrangeira, ou qualquer falha ao aplicar as mudanças no banco.

DbUpdateConcurrencyException: Lançada quando ocorre um conflito de concorrência durante uma operação de salvamento. Por exemplo, se dois usuários tentarem modificar a mesma entidade ao mesmo tempo.

DbEntityValidationException: Usada no EF6, ocorre quando as validações de dados falham. No EF Core, validações são feitas com ValidationAttribute.

InvalidOperationException: Ocorre em várias situações, como ao tentar acessar uma propriedade de navegação não carregada ou usar o contexto depois que ele foi descartado.

ArgumentNullException: Lançada quando um argumento obrigatório é null, como ao tentar adicionar uma entidade sem chave primária.

NotSupportedException: Ocorre quando uma operação não é suportada, como usar um método não traduzível para SQL em uma consulta LINQ.

SqlException: Derivada de erros específicos do SQL Server, como timeout, deadlock ou problemas de conexão.

Essas exceptions cobrem a maioria dos erros que você pode enfrentar ao usar o Entity Framework.
*/
