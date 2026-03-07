using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Domain.Errors;

public sealed record Error(string Code, string Description, params Error[] Errors)
{
    public static NotFound<Error> NotFound(string message)
        => TypedResults.NotFound(new Error(nameof(NotFound), message));

    public static BadRequest<Error> BadRequest(string message, params Error[] errors)
        => TypedResults.BadRequest(new Error(nameof(BadRequest), message, errors));

    public static Conflict<Error> Conflict(string message, params Error[] errors)
        => TypedResults.Conflict(new Error(nameof(Conflict), message, errors));

    public static UnprocessableEntity<Error> UnprocessableEntity(string message, params Error[] errors)
        => TypedResults.UnprocessableEntity(new Error(nameof(UnprocessableEntity), message, errors));

    public static ProblemHttpResult Forbidden(string message)
        => TypedResults.Problem(message, statusCode: StatusCodes.Status403Forbidden);
}
