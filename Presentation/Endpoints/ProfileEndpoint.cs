using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
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
    
    public static async Task<Results<Created<ProfileDtoResponse>, BadRequest<Error>>> CreateProfileAsync(
        [FromBody] ProfileDtoRequest dto,
        ProfileService service,
        ProfileErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        
        logger.CreatingStart(dto);
        
        var profile = await service.CreateAsync(dto);
        
        if (profile is null)
            return errors.Create();
            
        logger.Created(profile.Id);
        return TypedResults.Created(GetBaseEndpoint(profile), profile);
    }

    public static async Task<Results<Ok<ProfileDtoResponse>, NotFound<Error>, Conflict<Error>>> GetProfileAsync(
        int id,
        ProfileService service,
        ProfileErrors errors,
        UserService userService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.GettingStart(id);
        
        var profile = await service.GetByIdAsync(id);

        if (profile is null)
            return errors.NotFound();
            
        if (profile.Id != await userService.GetMyUserIdAsync())
            return errors.ProfileNotBelongsToUser();

        return TypedResults.Ok(profile);
    }

    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> UpdateProfileAsync(
        int id,
        [FromBody] ProfileDtoRequest dto,
        ProfileService service,
        ProfileErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        if (!await service.ProfileExistsAsync(id))
            return errors.NotFound();

        if (!await service.ProfileIsMineAsync(id))
            return errors.ProfileNotBelongsToUser();

        if (!await service.UpdateAsync(dto, id))
            return errors.Update();
            
        logger.Updated(id);
        return TypedResults.NoContent();
    }
}
