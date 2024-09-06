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

        group.MapPatch("{id}/image", UpdateProfileImage) // Endpoint para troca de imagem
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh))
            .DisableAntiforgery();;

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
        int userId = await userService.GetMyUserIdAsync();
        var profile = await service.GetByIdAsync(id, userId);

        if (profile is null)
            errors.ThrowNotFoundException();

        return Results.Ok(profile!.CreateDto());
    }

    public static async Task<IResult> CreateProfile(
        [FromBody] ProfileDtoCreate dto,
        IValidator<ProfileDtoCreate> validator,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors)
    {
        int userId = await userService.GetMyUserIdAsync();
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var profile = await service.CreateAsync(dto, userId);

        if (profile is null)
            errors.ThrowCreateException();

        return GetCreatedResult();
    }

    public static async Task<IResult> UpdateProfile(
        int id,
        [FromBody] ProfileDtoUpdate dto,
        IValidator<ProfileDtoUpdate> validator,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors,
        IUserErrors userErrors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int userId = await userService.GetMyUserIdAsync();
        var result = await service.UpdateAsync(dto, id, userId);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateProfileImage(
        int id,
        IFormFile image,
        IProfileService service,
        IUserService userService,
        IProfileErrors errors)
    {
        int userId = await userService.GetMyUserIdAsync();

        if (image == null || image.Length == 0)
            // errors.ThrowUpdateException("A imagem fornecida é inválida.");
            errors.ThrowUpdateException();

        var result = await service.UpdateProfileImageAsync(id, userId, image);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }
}
