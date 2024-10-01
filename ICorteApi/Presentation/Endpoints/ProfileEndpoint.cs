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
            .DisableAntiforgery();

        return app;
    }

    public static IResult GetCreatedResult()
    {
        string uri = EndpointPrefixes.User + "/me";
        object value = new { Message = "Pessoa criada com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateProfile(
        [FromBody] ProfileDtoCreate dto,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        await service.CreateAsync(dto, userId);
        return GetCreatedResult();
    }

    public static async Task<IResult> GetProfileById(
        int id,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        var profile = await service.GetByIdAsync(id, userId);
        return Results.Ok(profile);
    }

    public static async Task<IResult> UpdateProfile(
        int id,
        [FromBody] ProfileDtoUpdate dto,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, userId);
        return Results.NoContent();
    }

    public static async Task<IResult> UpdateProfileImage(
        int id,
        IFormFile image,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        await service.UpdateProfileImageAsync(id, userId, image);
        return Results.NoContent();
    }
}
