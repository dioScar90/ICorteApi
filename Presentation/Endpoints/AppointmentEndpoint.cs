using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class AppointmentEndpoint
{
    public static IEndpointRouteBuilder MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("appointment").WithTags("Appointment");

        group.MapPost("", CreateAppointmentAsync)
            .WithSummary("Create Appointment")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetAppointmentAsync)
            .WithSummary("Get Appointment")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllAppointmentsAsync)
            .WithSummary("Get All Appointments")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{id}", UpdateAppointmentAsync)
            .WithSummary("Update Appointment")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPatch("{id}", UpdatePaymentTypeAsync)
            .WithSummary("Update Payment Type")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete("{id}", DeleteAppointmentAsync)
            .WithSummary("Delete Appointment")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }
    
    private static IResult GetCreatedResult(AppointmentDtoResponse dto) =>
        Results.Created($"appointment/{dto.Id}", new { Message = "Agendamento criado com sucesso", Item = dto });

    public static async Task<IResult> CreateAppointmentAsync(
        AppointmentDtoRequest dto,
        AppointmentService service,
        UserService userService)
    {
        dto = dto with { ClientId = await userService.GetMyUserIdAsync() };
        var appointment = await service.CreateAsync(dto);
        return GetCreatedResult(appointment);
    }
    
    public static async Task<IResult> GetAppointmentAsync(
        int id,
        bool? services,
        AppointmentService service)
    {
        var appointment = await service.GetByIdAsync(id, new(services is true));
        
        return Results.Ok(appointment);
    }
    
    public static async Task<IResult> GetAllAppointmentsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        AppointmentService service,
        UserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var appointments = await service.GetAllAsync(page, pageSize, clientId);
        return Results.Ok(appointments);
    }

    public static async Task<IResult> UpdateAppointmentAsync(
        int id,
        AppointmentDtoRequest dto,
        AppointmentService service,
        UserService userService)
    {
        dto = dto with { ClientId = await userService.GetMyUserIdAsync() };

        await service.UpdateAsync(dto, id);
        return Results.NoContent();
    }
    
    public static async Task<IResult> UpdatePaymentTypeAsync(
        int id,
        AppointmentPaymentTypeDtoUpdateRequest dto,
        AppointmentService service,
        UserService userService)
    {
        dto = dto with { ClientId = await userService.GetMyUserIdAsync() };
        await service.UpdatePaymentTypeAsync(dto, id);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointmentAsync(
        int id,
        AppointmentService service,
        UserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, clientId);
        return Results.NoContent();
    }
}
