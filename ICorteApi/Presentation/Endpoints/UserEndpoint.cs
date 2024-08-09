using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Extensions;

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
        group.MapPost("", CreateUserWithPersonValues);
        group.MapPut("{id}", UpdateUser);
        group.MapDelete("{id}", DeleteUser);

        group.MapPut("{id}/roles/{role}/add", AddUserRole);
        group.MapPut("{id}/roles/{role}/remove", RemoveUserRole);
    }
    
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

        return Results.Ok(resp.Value!.CreateDto());
    }

    public static async Task<IResult> CreateUserWithPersonValues(
        int id,
        UserDtoRequest dto,
        IValidator<UserDtoRequest> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int userId = (await service.GetMeAsync()).Value!.Id;
        var res = await service.UpdateAsync(dto, userId);

        if (!res.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult();
    }

    public static async Task<IResult> AddUserRole(
        int id,
        UserRole role,
        IUserService service)
    {
        var response = await service.AddUserRoleAsync(role, id);

        if (!response.IsSuccess)
            return Results.BadRequest(response);

        return Results.NoContent();
    }

    public static async Task<IResult> RemoveUserRole(
        int id,
        UserRole role,
        IUserService service)
    {
        var response = await service.RemoveUserRoleAsync(role, id);

        if (!response.IsSuccess)
            return Results.BadRequest(response);

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateUser(
        int id,
        UserDtoRequest dto,
        IValidator<UserDtoRequest> validator,
        IUserService service,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.UpdateAsync(dto, id);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteUser(
        int id,
        IUserService service,
        IUserErrors errors)
    {
        var response = await service.DeleteAsync(id);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
