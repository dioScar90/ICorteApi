using FluentValidation;
using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Presentation.Endpoints;

public static class UserEndpoint
{
    public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("user").WithTags("User");
        
        group.MapGet("me", GetMeAsync)
            .WithSummary("Get Me")
            .WithDescription("If authenticated, you can get all basic information about your own user, such as user itself, profile, barber shop and roles.")
            .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

        group.MapPatch("changeEmail", UpdateUserEmailAsync)
            .WithSummary("Update User's Email")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPatch("changePassword", UpdateUserPasswordAsync)
            .WithSummary("Update User's Password")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPatch("changePhoneNumber", UpdateUserPhoneNumberAsync)
            .WithSummary("Update User's PhoneNumber")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete("", DeleteUserAsync)
            .WithSummary("Delete User")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }

    public static async Task<Ok<UserDtoResponse>> GetMeAsync(IUserService service, IUserErrors errors)
    {
        var user = await service.GetMeAsync();

        if (user is null)
            errors.ThrowNotFoundException();

        return TypedResults.Ok(user!.CreateDto());
    }

    public static async Task<IResult> UpdateUserEmailAsync(
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

    public static async Task<IResult> UpdateUserPasswordAsync(
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

    public static async Task<IResult> UpdateUserPhoneNumberAsync(
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

    public static async Task<IResult> DeleteUserAsync(IUserService service, IUserErrors errors)
    {
        int userId = await service.GetMyUserIdAsync();
        var result = await service.DeleteAsync(userId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
