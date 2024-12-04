using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class AdminEndpoint
{
    private const string CUSTOMIZED_HEADER_PASSPHRASE_NAME = "X-Admin-Passphrase";

    public static IEndpointRouteBuilder MapAdminEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("admin").WithTags("Admin");
        
        group.MapDelete("remove-all", RemoveAllRowsAsync)
            .WithSummary("Remove All Rows")
            .WithDescription("*** BEWARE *** - Remove all rows in all tables.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));
        
        group.MapDelete("remove-service", DeleteServiceAndRemoveFromAllAppointmentsAsync)
            .WithSummary("Delete Service And Remove From All Appointments")
            .WithDescription("*** BEWARE *** - Delete Service And Remove From All Appointments.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        group.MapPost("populate-all", PopulateAllInitialTablesAsync)
            .WithSummary("Populate All Initial Tables")
            .WithDescription("Populate tables with users, barber shops, services, schedules etc.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        group.MapPost("populate-appointments", PopulateWithAppointmentsAsync)
            .WithSummary("Populate With Appointments")
            .WithDescription("Populate Appointment table with many appointments based on existing users and barbers.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        group.MapPost("reset-password", ResetPasswordForSomeUserAsync)
            .WithSummary("Reset Password For Some User")
            .WithDescription("Reset the user's password to a known default password and then this user will be able to login again.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        group.MapGet("search-users", SearchForUsersByNameAsync)
            .WithSummary("Search For Users By Name")
            .WithDescription("Search for existing users in database and by some given name to be compared.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        group.MapGet("last-users", GetLastUsersAsync)
            .WithSummary("Get Last Users")
            .WithDescription("Get the last users created in the system.")
            .RequireAuthorization(nameof(PolicyUserRole.AdminOnly));

        return app;
    }
    
    private static async Task<string> GetCurrentUserEmail(this IUserService userService) => (await userService.GetMeAsync())?.Email ?? string.Empty;
    
    public static async Task<IResult> RemoveAllRowsAsync(
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)] string passphrase,
        [FromQuery] bool? evenMasterAdmin,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();

        await service.RemoveAllRows(passphrase, userEmail, evenMasterAdmin);

        return Results.NoContent();
    }
    
    public static async Task<IResult> DeleteServiceAndRemoveFromAllAppointmentsAsync(
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)] string passphrase,
        [FromQuery] int serviceId,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();

        await service.DeleteServiceAndRemoveFromAllAppointments(passphrase, userEmail, serviceId);

        return Results.NoContent();
    }
    
    public static async Task<IResult> PopulateAllInitialTablesAsync(
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)] string passphrase,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();

        await service.PopulateAllInitialTables(passphrase, userEmail);

        return Results.NoContent();
    }
    
    public static async Task<IResult> PopulateWithAppointmentsAsync(
        [FromQuery] DateOnly? firstDate,
        [FromQuery] DateOnly? limitDate,
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)] string passphrase,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();

        await service.PopulateWithAppointments(passphrase, userEmail, firstDate, limitDate);

        return Results.NoContent();
    }
    
    public static async Task<IResult> ResetPasswordForSomeUserAsync(
        ResetPasswordDto dto,
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)] string passphrase,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();

        await service.ResetPasswordForSomeUser(passphrase, userEmail, dto.Email);

        return Results.NoContent();
    }
    
    public record ResetPasswordDto(
        string Email
    );
    
    public static async Task<IResult> SearchForUsersByNameAsync(
        [FromQuery] string? q,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();
        
        var result = await service.SearchForUsersByName(userEmail, q);
        
        return Results.Ok(result);
    }
    
    public static async Task<IResult> GetLastUsersAsync(
        [FromQuery] int? take,
        IAdminService service,
        IUserService userService)
    {
        var userEmail = await userService.GetCurrentUserEmail();
        
        var result = await service.GetLastUsers(userEmail, take);
        
        return Results.Ok(result);
    }
}
