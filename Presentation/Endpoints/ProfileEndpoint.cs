using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ProfileEndpoint
{
    private static string GetBaseEndpoint(ProfileDtoResponse? profile = null) => profile is null
        ? "profile"
        : $"profile/{profile.Id}";

    public static IEndpointRouteBuilder MapProfileEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Profile");

        group.MapPost("", CreateProfileAsync)
            .WithSummary("Create Profile");

        group.MapGet("{id}", GetProfileAsync)
            .WithSummary("Get Profile")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{id}", UpdateProfileAsync)
            .WithSummary("Update Profile")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }
    
    internal record LoggerActions
    {
        private string Entity;
        private ILogger Logger;

        private LoggerActions() { }

        internal static LoggerActions FactoryCreate(ILoggerFactory loggerFactory)
        {
            return new()
            {
                Entity = nameof(Profile),
                Logger = loggerFactory.CreateLogger(nameof(ProfileEndpoint))
            };
        }
        
        internal void CreatingStart(ProfileDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);
            
        internal void UpdatingStart(int id, ProfileDtoRequest dto) =>
            Logger.LogInformation("Received request to update {Entity} with Id={Id} {@Entity}", Entity, id, dto);

        internal void Updated(int id) =>
            Logger.LogInformation("{Entity} successfully updated with Id={Id}", Entity, id);
    }
    
    public static async Task<Created<ProfileDtoResponse>> CreateProfileAsync(
        [FromBody] ProfileDtoRequest dto,
        ProfileService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.CreatingStart(dto);

        var profile = await service.CreateAsync(dto);

        logger.Created(profile.Id);
        return TypedResults.Created(GetBaseEndpoint(profile), profile);
    }

    public static async Task<Ok<ProfileDtoResponse>> GetProfileAsync(
        int id,
        ProfileService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var profile = await service.GetByIdAsync(id);
        return TypedResults.Ok(profile);
    }

    public static async Task<NoContent> UpdateProfileAsync(
        int id,
        [FromBody] ProfileDtoRequest dto,
        ProfileService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        await service.UpdateAsync(dto, id);

        logger.Updated(id);
        return TypedResults.NoContent();
    }
}
