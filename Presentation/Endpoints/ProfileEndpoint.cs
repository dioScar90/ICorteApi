using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ProfileEndpoint
{
    public static IEndpointRouteBuilder MapProfileEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("profile").WithTags("Profile");

        group.MapPost("", CreateProfileAsync)
            .WithSummary("Create Profile")
            .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

        group.MapGet("{id}", GetProfileAsync)
            .WithSummary("Get Profile")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{id}", UpdateProfileAsync)
            .WithSummary("Update Profile")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }

    public static IResult GetCreatedResult(ProfileDtoResponse dto) =>
        Results.Created("user/me", new { Message = "Pessoa criada com sucesso", Item = dto });

    public static async Task<IResult> CreateProfileAsync(
        [FromBody] ProfileDtoCreate dto,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        var profile = await service.CreateAsync(dto, userId);
        return GetCreatedResult(profile);
    }

    public static async Task<IResult> GetProfileAsync(
        int id,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        var profile = await service.GetByIdAsync(id, userId);
        return Results.Ok(profile);
    }

    public static async Task<IResult> UpdateProfileAsync(
        int id,
        [FromBody] ProfileDtoUpdate dto,
        IProfileService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, userId);
        return Results.NoContent();
    }
}
