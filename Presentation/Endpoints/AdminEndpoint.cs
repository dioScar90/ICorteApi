using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    private static async Task<string> GetCurrentUserEmail(this UserService userService) =>
        (await userService.GetMeAsync())?.Email ?? string.Empty;
    
    public static async Task<Results<NoContent, BadRequest<Error>, Conflict<Error>>> RemoveAllRowsAsync(
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)]
        [Required]
        string passphrase,
        
        [FromQuery]
        bool? evenMasterAdmin,
        
        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();

        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
            
        if (!service.IsCorrectAdminPassphrase(passphrase))
            return errors.NotEqualPassphase();
            
        if (!await service.IsThereAnyUserHere(evenMasterAdmin is true, cancellationToken))
            return errors.ThereIsNobodyToBeDeleted();
            
        await service.RemoveAllRows(userEmail, evenMasterAdmin is true, cancellationToken);
        
        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, Conflict<Error>>> DeleteServiceAndRemoveFromAllAppointmentsAsync(
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)]
        [Required]
        string passphrase,

        [FromQuery] int serviceId,
        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();

        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
            
        if (!service.IsCorrectAdminPassphrase(passphrase))
            return errors.NotEqualPassphase();

        await service.DeleteServiceAndRemoveFromAllAppointments(serviceId, cancellationToken);

        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, BadRequest<Error>, Conflict<Error>>> PopulateAllInitialTablesAsync(
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)]
        [Required]
        string passphrase,

        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();

        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
            
        if (!service.IsCorrectAdminPassphrase(passphrase))
            return errors.NotEqualPassphase();
            
        if (!await service.IsThereAnyUserHere(false, cancellationToken))
            return errors.ThereAreTooManyPeopleHere();

        await service.PopulateAllInitialTables(cancellationToken);

        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, BadRequest<Error>, Conflict<Error>>> PopulateWithAppointmentsAsync(
        [FromQuery]
        DateOnly? firstDate,
        [FromQuery]
        DateOnly? limitDate,
        
        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)]
        [Required]
        string passphrase,

        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();

        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
            
        if (!service.IsCorrectAdminPassphrase(passphrase))
            return errors.NotEqualPassphase();
            
        PeriodToPopulateDto dto = new(firstDate, limitDate);
        
        if (dto.DayToPopulate > dto.VeryLimitDate)
            return errors.LimitDateIsLessThanStartDate();
            
        if (!await service.IsThereAnyUserHere(false, cancellationToken))
            return errors.ThereAreTooManyPeopleHere();
            
        if (await service.IsThereAnyAppointmentHere(dto, cancellationToken))
            return errors.ThereAreTooManyAppointmentsHere();
            
        await service.PopulateWithAppointments(dto, cancellationToken);

        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, BadRequest<Error>, Conflict<Error>, NotFound<Error>>> ResetPasswordForSomeUserAsync(
        ResetPasswordDto dto,

        [FromHeader(Name = CUSTOMIZED_HEADER_PASSPHRASE_NAME)]
        [Required]
        string passphrase,

        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();

        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
            
        if (!service.IsCorrectAdminPassphrase(passphrase))
            return errors.NotEqualPassphase();
            
        if (!await service.UserExists(dto.Email, cancellationToken))
            return errors.UserDoesNotExist(dto.Email);
            
        var identityResult = await service.ResetPasswordForSomeUser(dto.Email, cancellationToken);

        if (identityResult is null)
            return errors.UserDoesNotExist(dto.Email);
        
        if (!identityResult.Succeeded)
            return errors.ResetPassword(dto.Email, [..identityResult.Errors]);

        return TypedResults.NoContent();
    }
    
    public static async Task<Results<Ok<FoundUserByAdmin[]>, Conflict<Error>>> SearchForUsersByNameAsync(
        [FromQuery] string? q,
        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();

        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
            
        var result = await service.SearchForUsersByName(q, cancellationToken);
        
        return TypedResults.Ok(result);
    }
    
    public static async Task<Results<Ok<FoundUserByAdmin[]>, Conflict<Error>>> GetLastUsersAsync(
        [FromQuery] int? take,
        AdminService service,
        UserService userService,
        AdminErrors errors,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = await userService.GetCurrentUserEmail();
        
        if (!service.IsAllowableAdminEmail(userEmail))
            return errors.NotEqualEmail();
        
        var result = await service.GetLastUsers(take, cancellationToken);
        
        return TypedResults.Ok(result);
    }
}
