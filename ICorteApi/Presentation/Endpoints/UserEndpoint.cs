using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Identity;

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

        group.MapPost("register", RegisterUser)
            .AllowAnonymous();

        return app;
    }

    public static IResult GetCreatedResult()
    {
        string uri = EndpointPrefixes.User + "/me";
        object value = new { Message = "Usuário criado com sucesso" };
        return Results.Created(uri, value);
    }

    // This method was written using both inspiration of Chat GPT and real Microsoft ASP.NET Core documentation,
    // that you can find in: https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
    public static async Task<IResult> RegisterUser(
        UserDtoRegisterRequest dto,
        IValidator<UserDtoRegisterRequest> validator,
        IUserService service,
        UserManager<User> userManager,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var user = await service.CreateAsync(dto);

        if (user is null)
            errors.ThrowCreateException();

        return GetCreatedResult();
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
        int userId = service.GetMyUserId();
        var result = await service.DeleteAsync(userId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
