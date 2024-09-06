using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Extensions;
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

    private static async Task<IResult> LoginAfterCreated(string userName, string password, bool useCookies, SignInManager<User> signInManager)
    {
        var result = await signInManager.PasswordSignInAsync(userName, password, useCookies, lockoutOnFailure: true);
        
        if (!result.Succeeded)
            return Results.Unauthorized();
        
        return GetCreatedResult();




        // var signInManager = sp.GetRequiredService<SignInManager<TUser>>();

        // var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        // var isPersistent = (useCookies == true) && (useSessionCookies != true);
        // signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        // var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

        // if (result.RequiresTwoFactor)
        // {
        //     if (!string.IsNullOrEmpty(login.TwoFactorCode))
        //     {
        //         result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
        //     }
        //     else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
        //     {
        //         result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
        //     }
        // }

        // if (!result.Succeeded)
        // {
        //     return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        // }

        // // The signInManager already produced the needed response in the form of a cookie or bearer token.
        // return TypedResults.Empty;
    }

    // This method was written using both inspiration of Chat GPT and real Microsoft ASP.NET Core documentation,
    // that you can find in: https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
    public static async Task<IResult> RegisterUser(
        [FromQuery] bool useCookies,
        UserDtoRegisterRequest dto,
        IValidator<UserDtoRegisterRequest> validator,
        IUserService service,
        SignInManager<User> signInManager,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var user = await service.CreateAsync(dto);

        if (user is null)
            errors.ThrowCreateException();

        return await LoginAfterCreated(dto.Email, dto.Password, useCookies, signInManager);

        // if (useCookies)
        //     Login(dto.Email, dto.Password, userCookies);

        // return GetCreatedResult();
    }

    public static async Task<IResult> GetMe(IUserService service, IUserErrors errors)
    {
        var user = await service.GetMeAsync();

        if (user is null)
            errors.ThrowNotFoundException();
        
        return Results.Ok(user!.CreateDto());
    }

    public static async Task<IResult> UpdateUserEmail(
        UserDtoChangeEmailRequest dto,
        IValidator<UserDtoChangeEmailRequest> validator,
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
        UserDtoChangePasswordRequest dto,
        IValidator<UserDtoChangePasswordRequest> validator,
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
        UserDtoChangePhoneNumberRequest dto,
        IValidator<UserDtoChangePhoneNumberRequest> validator,
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
