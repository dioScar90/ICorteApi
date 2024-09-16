using FluentValidation;
using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        group.MapPost("register", RegisterUser)
            .AllowAnonymous();

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

    public static IResult GetCreatedResult()
    {
        string uri = EndpointPrefixes.User + "/me";
        object value = new { Message = "Usuário criado com sucesso" };
        return Results.Created(uri, value);
    }

    private static async Task<IResult> LoginAfterCreated(string userName, string password, SignInManager<User> signInManager)
    {
        const bool USE_COOKIES = true;
        var result = await signInManager.PasswordSignInAsync(userName, password, USE_COOKIES, lockoutOnFailure: true);

        if (!result.Succeeded)
            return Results.Unauthorized();

        return GetCreatedResult();
    }

    // This method was written using both inspiration of Chat GPT and real Microsoft ASP.NET Core documentation,
    // that you can find in: https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
    public static async Task<IResult> RegisterUser(
        UserDtoRegisterCreate dto,
        IValidator<UserDtoRegisterCreate> validator,
        IUserService service,
        SignInManager<User> signInManager,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var user = await service.CreateAsync(dto);

        if (user is null)
            errors.ThrowCreateException();

        return await LoginAfterCreated(dto.Email, dto.Password, signInManager);
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
