using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class UserEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.User;
    private static readonly string ENDPOINT_NAME = EndpointNames.User;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet("me", GetMe);
        group.MapPost(INDEX, CreateUserWithPersonValues);
        group.MapPut(INDEX, UpdateUser);
        group.MapDelete(INDEX, DeleteUser);

        group.MapGet("roles", GetUserRoles);
        group.MapPost("roles/{role}", AddUserRole);
        group.MapDelete("roles/{role}", RemoveUserRole);
    }

    private static int GetMyUserId(this IUserService service) => (int)service.GetMyUserIdAsync()!;
    private static async Task<UserRole[]> GetMyUserRoles(this IUserService service) => await service.GetUserRolesAsync() ?? [];
    
    public static IResult GetCreatedResult()
    {
        string uri = EndpointPrefixes.BarberShop + "/me";
        object value = new { Message = "Usuário criado com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> GetMe(
        IUserService service,
        IUserErrors errors)
    {
        var resp = await service.GetMeAsync();

        if (!resp.IsSuccess)
            errors.ThrowNotFoundException();

        if (!resp.Value!.IsRegisterCompleted)
            errors.ThrowRegisterNotCompletedException();

        return Results.Ok(resp.Value!.CreateDto());
    }

    public static async Task<IResult> CreateUserWithPersonValues(
        UserDtoRequest dto,
        IValidator<UserDtoRequest> validator,
        IUserService service,
        IUserErrors errors)
    {
        var user = (await service.GetMeAsync()).Value;

        if (user is null)
            errors.ThrowBadRequestException();
        
        if (user!.IsRegisterCompleted)
            errors.ThrowUserAlreadyCreatedException();
        
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var res = await service.UpdateAsync(dto, user.Id);

        if (!res.IsSuccess)
            errors.ThrowCreateException();
            
        return GetCreatedResult();
    }

    public static async Task<IResult> GetUserRoles(IUserService service)
    {
        var userRoles = await service.GetMyUserRoles();
        return Results.Ok(new { userRoles });
    }

    public static async Task<IResult> AddUserRole(
        UserRole role,
        IUserService service)
    {
        int userId = service.GetMyUserId();
        var response = await service.AddUserRoleAsync(role, userId);

        if (!response.IsSuccess)
            return Results.BadRequest(response);

        return Results.NoContent();
    }

    public static async Task<IResult> RemoveUserRole(
        UserRole role,
        IUserService service)
    {
        int userId = service.GetMyUserId();
        var response = await service.RemoveUserRoleAsync(role, userId);

        if (!response.IsSuccess)
            return Results.BadRequest(response);

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateUser(
        UserDtoRequest dto,
        IValidator<UserDtoRequest> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int userId = service.GetMyUserId();
        var response = await service.UpdateAsync(dto, userId);

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
