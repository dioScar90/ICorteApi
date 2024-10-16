namespace ICorteApi.Presentation.Endpoints;

public static class AdminEndpoint
{
    public static IEndpointRouteBuilder MapAdminEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("admin").WithTags("Admin");

        group.MapPost("remove-all", RemoveAllRows)
            .WithSummary("Remove All Rows")
            .WithDescription("*** BEWARE *** - Remove all rows in all tables.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        return app;
    }
    
    public static async Task<IResult> RemoveAllRows(AdminRemoveAllDto dto, IAdminService service, IUserService userService)
    {
        var user = await userService.GetMeAsync();
        await service.RemoveAllRows(dto.Passphrase, user?.Email ?? "");
        return Results.NoContent();
    }

    public record AdminRemoveAllDto(string Passphrase);
}
