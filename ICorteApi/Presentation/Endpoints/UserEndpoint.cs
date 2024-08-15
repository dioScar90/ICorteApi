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
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.Free));

        group.MapPost("register", RegisterUser)
            .AllowAnonymous();

        group.MapGet("me", GetMe);
        group.MapPatch("changeEmail", UpdateUserEmail);
        group.MapPatch("changePassword", UpdateUserPassword);
        group.MapPatch("changePhoneNumber", UpdateUserPhoneNumber);
        group.MapDelete(INDEX, DeleteUser);
        
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

        var result = await service.CreateAsync(dto);
        
        if (!result.IsSuccess)
            errors.ThrowCreateException();
            
        return GetCreatedResult();
    }

    public static async Task<IResult> GetMe(
        IUserService service,
        IUserErrors errors)
    {
        var resp = await service.GetMeAsync();

        if (!resp.IsSuccess)
            errors.ThrowNotFoundException();
        
        var dto = resp.Value!.CreateDto();
        return Results.Ok(dto with { Roles = await service.GetUserRolesAsync() });
    }
    
    public static async Task<IResult> UpdateUserEmail(
        UserDtoChangeEmailRequest dto,
        IValidator<UserDtoChangeEmailRequest> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.UpdateEmailAsync(dto);

        if (!response.IsSuccess)
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

        var response = await service.UpdatePasswordAsync(dto);

        if (!response.IsSuccess)
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

        var response = await service.UpdatePhoneNumberAsync(dto);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteUser(IUserService service, IUserErrors errors)
    {
        int userId = service.GetMyUserId();
        var response = await service.DeleteAsync(userId);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
