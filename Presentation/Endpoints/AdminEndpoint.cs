using Microsoft.AspNetCore.Mvc;

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
    
    public static async Task<IResult> RemoveAllRows(
        AdminRemoveAllDto dto,
        [FromQuery] bool? evenMasterAdmin,
        IAdminService service,
        IUserService userService)
    {
        var user = await userService.GetMeAsync();
        var userEmail = user?.Email ?? string.Empty;

        await service.RemoveAllRows(dto.Passphrase, userEmail, evenMasterAdmin);

        return Results.NoContent();
    }

    public record AdminRemoveAllDto(string Passphrase);
}
