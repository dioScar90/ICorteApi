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
        
        if (exception is UnprocessableEntity br && br.Errors is not null)
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
}

/*
Aqui está uma lista dos retornos HTTP mais comuns e seus respectivos status codes:

Item criado com sucesso
Status Code: 201 Created
Descrição: Indica que a requisição foi bem-sucedida e resultou na criação de um recurso.

Item atualizado com sucesso
Status Code: 204 No Content
Descrição: Indica que a requisição foi bem-sucedida e o recurso foi atualizado, mas não há conteúdo adicional a ser enviado no corpo da resposta.

Item encontrado com sucesso
Status Code: 200 OK
Descrição: Indica que a requisição foi bem-sucedida e o recurso solicitado está sendo retornado no corpo da resposta.

Objeto enviado no request possui divergências (faltando campos etc.)
Status Code: 400 Bad Request
Descrição: Indica que o servidor não pôde processar a requisição devido a um erro do cliente (por exemplo, campos obrigatórios ausentes).

Objeto enviado no request possui campos que não passaram na validação (por exemplo, enviar "age" com value "cachorro")
Status Code: 422 Unprocessable Entity
Descrição: Indica que o servidor entende o tipo de conteúdo da entidade de requisição e a sintaxe da entidade está correta, mas não consegue processar as instruções contidas.

Recurso não encontrado
Status Code: 404 Not Found
Descrição: Indica que o recurso solicitado não pôde ser encontrado, mas pode estar disponível no futuro.

Autenticação necessária
Status Code: 401 Unauthorized
Descrição: Indica que a requisição não foi aplicada porque não possui credenciais de autenticação válidas para o recurso alvo.

Acesso negado
Status Code: 403 Forbidden
Descrição: Indica que o servidor entendeu a requisição, mas se recusa a autorizá-la.

Erro interno no servidor
Status Code: 500 Internal Server Error
Descrição: Indica que o servidor encontrou uma condição inesperada que o impediu de atender à requisição.

Requisição redirecionada
Status Code: 302 Found (ou 301 Moved Permanently para redirecionamento permanente)
Descrição: Indica que a resposta a esta requisição pode ser encontrada em outra URI, a qual deve ser recebida com uma requisição GET.

Método não permitido
Status Code: 405 Method Not Allowed
Descrição: Indica que o método recebido na linha de requisição é conhecido pelo servidor mas não é suportado pelo recurso alvo.

Conflito
Status Code: 409 Conflict
Descrição: Indica que a requisição não pôde ser processada por causa de um conflito no estado atual do recurso.

Requisição bem-sucedida, mas sem conteúdo relevante
Status Code: 204 No Content
Descrição: Indica que a requisição foi bem-sucedida, mas o servidor não está retornando nenhum conteúdo.

Requisição aceita para processamento, mas não concluída
Status Code: 202 Accepted
Descrição: Indica que a requisição foi aceita para processamento, mas o processamento não foi concluído.

Espero que essas informações sejam úteis para a implementação dos retornos HTTP apropriados no seu sistema.
*/