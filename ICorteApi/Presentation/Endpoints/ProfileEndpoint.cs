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

        group.MapPut(INDEX, UpdateProfile)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete(INDEX, DeleteProfile)
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
        IProfileErrors errors)
    {
        var resp = await service.GetByIdAsync(id);

        if (!resp.IsSuccess)
            errors.ThrowNotFoundException();

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

        var res = await service.CreateAsync(dto, userId);

        if (!res.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult();
    }

    public static async Task<IResult> UpdateProfile(
        [FromBody] ProfileDtoRequest dto,
        IValidator<ProfileDtoRequest> validator,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors)
    {
        int userId = userService.GetMyUserId();
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var res = await service.UpdateAsync(dto, userId);

        if (!res.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteProfile(
        IProfileService service,
        IUserService userService,
        IProfileErrors errors)
    {
        int userId = userService.GetMyUserId();
        var response = await service.DeleteAsync(userId);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
