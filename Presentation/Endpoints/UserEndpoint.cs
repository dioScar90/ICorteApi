using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Presentation.Endpoints;

public static class UserEndpoint
{
    private static string GetBaseEndpoint(UserDtoResponse? user = null) => user is null
        ? "user"
        : $"user/{user.Id}";

    public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("User");
        
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
            
        group.MapPut("roles", AddUserRoleAsync)
            .WithSummary("Add User To Role")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
            
        group.MapDelete("roles", RemoveFromRoleAsync)
            .WithSummary("Remove From Role")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete("{id}", DeleteUserAsync)
            .WithSummary("Delete User")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }
    
    public static async Task<Results<Ok<UserDtoResponse>, NotFound<Error>>> GetMeAsync(UserService service, UserErrors errors)
    {
        var user = await service.GetMeAsync();

        if (user is null)
            return errors.NotFound();
            
        return TypedResults.Ok(user);
    }

    public static async Task<Results<NoContent, BadRequest<Error>, UnprocessableEntity<Error>, ProblemHttpResult>> UpdateUserEmailAsync(
        UserDtoEmailUpdate dto,
        UserService service,
        UserErrors errors)
    {
        if (!await service.IsUserFromGivenId(dto.Id))
            return errors.WrongUserId(dto.Id);

        var result = await service.UpdateEmailAsync(dto);

        if (result is null)
            return errors.Update();
            
        if (!result.Succeeded)
            return errors.UpdateEmail([..result.Errors]);
            
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest<Error>, UnprocessableEntity<Error>, ProblemHttpResult>> UpdateUserPasswordAsync(
        UserDtoPasswordUpdateRequest dto,
        UserService service,
        UserErrors errors)
    {
        if (!await service.IsUserFromGivenId(dto.Id))
            return errors.WrongUserId(dto.Id);

        var result = await service.UpdatePasswordAsync(dto);

        if (result is null)
            return errors.Update();
            
        if (!result.Succeeded)
            return errors.UpdatePassword([..result.Errors]);
            
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest<Error>, UnprocessableEntity<Error>, ProblemHttpResult>> UpdateUserPhoneNumberAsync(
        UserDtoPhoneNumberUpdate dto,
        UserService service,
        UserErrors errors)
    {
        if (!await service.IsUserFromGivenId(dto.Id))
            return errors.WrongUserId(dto.Id);

        var result = await service.UpdatePhoneNumberAsync(dto);

        if (result is null)
            return errors.Update();
            
        if (!result.Succeeded)
            return errors.UpdatePhoneNumber([..result.Errors]);
            
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest<Error>, UnprocessableEntity<Error>, ProblemHttpResult>> AddUserRoleAsync(
        UserDtoAddRoleRequest dto,
        UserService service,
        UserErrors errors)
    {
        if (!await service.IsUserFromGivenId(dto.Id))
            return errors.WrongUserId(dto.Id);

        var result = await service.AddUserRoleAsync(dto);

        if (result is null)
            return errors.Update();
            
        if (!result.Succeeded)
            return errors.AddUserRole([..result.Errors]);
            
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest<Error>, UnprocessableEntity<Error>, ProblemHttpResult>> RemoveFromRoleAsync(
        UserDtoRemoveRoleRequest dto,
        UserService service,
        UserErrors errors)
    {
        if (!await service.IsUserFromGivenId(dto.Id))
            return errors.WrongUserId(dto.Id);
            
        var result = await service.RemoveFromRoleAsync(dto);

        if (result is null)
            return errors.Update();
            
        if (!result.Succeeded)
            return errors.RemoveUserRole([..result.Errors]);
            
        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, BadRequest<Error>, UnprocessableEntity<Error>, ProblemHttpResult>> DeleteUserAsync(
        int id,
        UserService service,
        UserErrors errors)
    {
        if (!await service.IsUserFromGivenId(id))
            return errors.WrongUserId(id);

        var result = await service.DeleteAsync(id);

        if (result is null)
            return errors.Delete();

        if (!result.Succeeded)
            return errors.BasicUser([..result.Errors]);

        return TypedResults.NoContent();
    }
}
