using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class PersonEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.User + "/{userId}/" + EndpointPrefixes.Person;
    private static readonly string ENDPOINT_NAME = EndpointNames.Person;

    public static IEndpointRouteBuilder MapPersonEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.Free));

        group.MapPost(INDEX, CreatePerson);
        group.MapGet(INDEX, GetPersonById);
        group.MapPut(INDEX, UpdatePerson);
        group.MapDelete(INDEX, DeletePerson);

        return app;
    }
    
    public static IResult GetCreatedResult()
    {
        string uri = EndpointPrefixes.User + "/me";
        object value = new { Message = "Pessoa criada com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> GetPersonById(
        int userId,
        IPersonService service,
        IUserService userService,
        IPersonErrors errors,
        IUserErrors userErrors)
    {
        var resp = await userService.GetMeAsync();

        if (!resp.IsSuccess)
            errors.ThrowNotFoundException();
        
        return Results.Ok(resp.Value!.CreateDto());
    }

    public static async Task<IResult> CreatePerson(
        int userId,
        [FromBody] PersonDtoRegisterRequest dto,
        IValidator<PersonDtoRegisterRequest> validator,
        IPersonService service,
        IUserService userService,
        IPersonErrors errors,
        IUserErrors userErrors)
    {
        if (userService.GetMyUserId() != userId)
            userErrors.ThrowWrongUserIdException(userId);
        
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var res = await service.CreateAsync(dto, userId);

        if (!res.IsSuccess)
            errors.ThrowCreateException();
        
        return GetCreatedResult();
    }

    public static async Task<IResult> UpdatePerson(
        int userId,
        [FromBody] PersonDtoRequest dto,
        IValidator<PersonDtoRequest> validator,
        IPersonService service,
        IUserService userService,
        IPersonErrors errors,
        IUserErrors userErrors)
    {
        if (userService.GetMyUserId() != userId)
            userErrors.ThrowWrongUserIdException(userId);
        
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var res = await service.UpdateAsync(dto, userId);

        if (!res.IsSuccess)
            errors.ThrowUpdateException();
        
        return Results.NoContent();
    }
    
    public static async Task<IResult> DeletePerson(
        int userId,
        IPersonService service,
        IUserService userService,
        IPersonErrors errors,
        IUserErrors userErrors)
    {
        if (userService.GetMyUserId() != userId)
            userErrors.ThrowWrongUserIdException(userId);
        
        var response = await service.DeleteAsync(userId);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();
            
        // Remove o role de BarberShop
        var roleResult = await userService.RemoveFromRoleAsync(UserRole.Client);

        if (!roleResult.IsSuccess)
            errors.ThrowBadRequestException();

        return Results.NoContent();
    }
}
