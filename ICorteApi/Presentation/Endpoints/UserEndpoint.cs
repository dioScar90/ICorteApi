using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Endpoints;

public static class UserEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.User;
    private static readonly string ENDPOINT_NAME = EndpointNames.User;

    public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);
            
        group.MapGet("me", GetMe)
            .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

        group.MapPatch("changeEmail", UpdateUserEmail)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPatch("changePassword", UpdateUserPassword)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPatch("changePhoneNumber", UpdateUserPhoneNumber)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete(INDEX, DeleteUser)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }

    public static async Task<IResult> GetMe(IUserService service, IUserErrors errors)
    {
        var user = await service.GetMeAsync();

        if (user is null)
            errors.ThrowNotFoundException();

        return Results.Ok(user!.CreateDto());
    }

    public static async Task<IResult> UpdateUserEmail(
        UserDtoEmailUpdate dto,
        IValidator<UserDtoEmailUpdate> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var result = await service.UpdateEmailAsync(dto);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateUserPassword(
        UserDtoPasswordUpdate dto,
        IValidator<UserDtoPasswordUpdate> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var result = await service.UpdatePasswordAsync(dto);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateUserPhoneNumber(
        UserDtoPhoneNumberUpdate dto,
        IValidator<UserDtoPhoneNumberUpdate> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var result = await service.UpdatePhoneNumberAsync(dto);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteUser(IUserService service, IUserErrors errors)
    {
        int userId = await service.GetMyUserIdAsync();
        var result = await service.DeleteAsync(userId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
