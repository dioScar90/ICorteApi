﻿using ICorteApi.Domain.Interfaces;
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
    
    public static IResult GetCreatedResult(AppointmentDtoResponse dto) =>
        Results.Created($"appointment/{dto.Id}", new { Message = "Agendamento criado com sucesso", Item = dto });

    public static async Task<IResult> CreateAppointmentAsync(
        AppointmentDtoCreate dto,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var appointment = await service.CreateAsync(dto, clientId);
        return GetCreatedResult(appointment);
    }
    
    public static async Task<IResult> GetAppointmentAsync(
        int id,
        bool? services,
        IAppointmentService service)
    {
        var appointment = services is true ? await service.GetByIdWithServicesAsync(id) : await service.GetByIdAsync(id);
        return Results.Ok(appointment);
    }
    
    public static async Task<IResult> GetAllAppointmentsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var appointments = await service.GetAllAsync(page, pageSize, clientId);
        return Results.Ok(appointments);
    }

    public static async Task<IResult> UpdateAppointmentAsync(
        int id,
        AppointmentDtoUpdate dto,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, clientId);
        return Results.NoContent();
    }
    
    public static async Task<IResult> UpdatePaymentTypeAsync(
        int id,
        AppointmentPaymentTypeDtoUpdate dto,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.UpdatePaymentTypeAsync(dto, id, clientId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointmentAsync(
        int id,
        IAppointmentService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, clientId);
        return Results.NoContent();
    }
}
