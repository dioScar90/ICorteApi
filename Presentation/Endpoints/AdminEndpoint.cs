namespace ICorteApi.Presentation.Endpoints;

public static class AdminEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Admin;
    private static readonly string ENDPOINT_NAME = EndpointNames.Admin;

    public static IEndpointRouteBuilder MapAdminEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost("remove-all", RemoveAllRows)
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly))
            .WithDescription("Remove all rows in all tables - *** BEWARE ***");

        return app;
    }
    
    public static async Task<IResult> RemoveAllRows(AdminRemoveAllDto dto, IAdminService service, IUserService userService)
    {
        var user = await userService.GetMeAsync();
        await service.RemoveAllRows(dto.Passphrase, user?.Email ?? "");
        return Results.NoContent();
    }
}

public record AdminRemoveAllDto(
    string Passphrase
);
