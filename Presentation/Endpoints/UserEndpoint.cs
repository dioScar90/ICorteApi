using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Presentation.Endpoints;

public static class UserEndpoint
{
    public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("user").WithTags("User");
        
        group.MapGet("me", GetMeAsync)
            .WithSummary("Get Me")
            .WithDescription("If authenticated, you can get all basic information about your own user, such as user itself, profile, barber shop and roles.");

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
    
    public static async Task<Ok<UserDtoResponse>> GetMeAsync(UserService service, UserErrors errors)
    {
        var user = await service.GetMeAsync();

        if (user is null)
            errors.ThrowNotFoundException();

        return TypedResults.Ok(user!.CreateDto());
    }

    public static async Task<IResult> UpdateUserEmailAsync(
        UserDtoEmailUpdate dto,
        UserService service,
        UserErrors errors)
    {
        var result = await service.UpdateEmailAsync(dto);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateUserPasswordAsync(
        UserDtoPasswordUpdateRequest dto,
        UserService service,
        UserErrors errors)
    {
        await service.UpdatePasswordAsync(dto);
        return Results.NoContent();
    }

    public static async Task<IResult> UpdateUserPhoneNumberAsync(
        UserDtoPhoneNumberUpdate dto,
        UserService service,
        UserErrors errors)
    {
        await service.UpdatePhoneNumberAsync(dto);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteUserAsync(
        UserService service,
        UserErrors errors)
    {
        await service.DeleteAsync(await service.GetMyUserIdAsync());
        return Results.NoContent();
    }
}
