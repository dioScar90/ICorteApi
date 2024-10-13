using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class AppointmentEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Appointment;
    private static readonly string ENDPOINT_NAME = EndpointNames.Appointment;

    public static IEndpointRouteBuilder MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost(INDEX, CreateAppointment)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetAppointment)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllAppointments)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{id}", UpdateAppointment)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete("{id}", DeleteAppointment)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(int newId)
    {
        string uri = EndpointPrefixes.Appointment + "/" + newId;
        object value = new { Message = "Agendamento criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateAppointment(
        AppointmentDtoCreate dto,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var appointment = await service.CreateAsync(dto, clientId);
        return GetCreatedResult(appointment.Id);
    }

    public static async Task<IResult> GetAppointment(
        int id,
        IAppointmentService service)
    {
        var appointment = await service.GetByIdAsync(id);
        return Results.Ok(appointment);
    }

    public static async Task<IResult> GetAllAppointments(
        int clientId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        IAppointmentService service,
        IAppointmentErrors errors)
    {
        var appointments = await service.GetAllAsync(page, pageSize, clientId);
        return Results.Ok(appointments);
    }

    public static async Task<IResult> UpdateAppointment(
        int id,
        AppointmentDtoUpdate dto,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, clientId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointment(
        int id,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, clientId);
        return Results.NoContent();
    }
}
