using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ProfileEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Profile;
    private static readonly string ENDPOINT_NAME = EndpointNames.Profile;

    public static IEndpointRouteBuilder MapProfileEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);
            
        group.MapPost(INDEX, CreateProfile)
            .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

        group.MapGet("{id}", GetProfileById)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{id}", UpdateProfile)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
            
        return app;
    }

    public static IResult GetCreatedResult()
    {
        string uri = EndpointPrefixes.User + "/me";
        object value = new { Message = "Pessoa criada com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetProfileById(
        int id,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors)
    {
        int userId = userService.GetMyUserId();
        var resp = await service.GetByIdAsync(id, userId);

        if (!resp.IsSuccess)
            errors.ThrowNotFoundException(resp.Error);

        return Results.Ok(resp.Value!.CreateDto());
    }

    public static async Task<IResult> CreateProfile(
        [FromBody] ProfileDtoRegisterRequest dto,
        IValidator<ProfileDtoRegisterRequest> validator,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors)
    {
        int userId = userService.GetMyUserId();
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var resp = await service.CreateAsync(dto, userId);

        if (!resp.IsSuccess)
            errors.ThrowCreateException(resp.Error);

        return GetCreatedResult();
    }

    public static async Task<IResult> UpdateProfile(
        int id,
        [FromBody] ProfileDtoRequest dto,
        IValidator<ProfileDtoRequest> validator,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors,
        IUserErrors userErrors)
    {
        int userId = userService.GetMyUserId();

        if (userId != id)
            userErrors.ThrowWrongUserIdException(id);
        
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var resp = await service.UpdateAsync(dto, id, userId);

        if (!resp.IsSuccess)
            errors.ThrowUpdateException(resp.Error);

        return Results.NoContent();
    }
}
