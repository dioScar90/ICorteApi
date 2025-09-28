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
    
    internal record LoggerActions
    {
        private string Entity;
        private ILogger Logger;

        private LoggerActions() { }

        internal static LoggerActions FactoryCreate(ILoggerFactory loggerFactory)
        {
            return new()
            {
                Entity = nameof(Appointment),
                Logger = loggerFactory.CreateLogger(nameof(AppointmentEndpoint))
            };
        }

        internal void CreatingStart(AppointmentDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);

        internal void GettingAllStart(int? page, int? pageSize) =>
            Logger.LogInformation("Received request to get all {Entity} with Page={Page} PageSize={PageSize}", Entity, page, pageSize);

        internal void UpdatingStart(int id, AppointmentDtoRequest dto) =>
            Logger.LogInformation("Received request to update {Entity} with Id={Id} {@Entity}", Entity, id, dto);

        internal void Updated(int id) =>
            Logger.LogInformation("{Entity} successfully updated with Id={Id}", Entity, id);

        internal void UpdatingPaymentStart(int id, AppointmentPaymentTypeDtoUpdateRequest dto) =>
            Logger.LogInformation(
                "Received request to update payment {Entity} with Id={Id} {@Entity}",
                Entity, id, dto);

        internal void UpdatedPayment(int id) =>
            Logger.LogInformation("{Entity} successfully updated payment with Id={Id}", Entity, id);

        internal void DeletingStart(int id) =>
            Logger.LogInformation("Received request to delete {Entity} Id={Id}", Entity, id);

        internal void Deleted(int id) =>
            Logger.LogInformation("{Entity} successfully deleted with Id={Id}", Entity, id);
    }
    
    private static IResult GetCreatedResult(AppointmentDtoResponse dto) =>
        Results.Created($"appointment/{dto.Id}", new { Message = "Agendamento criado com sucesso", Item = dto });

    public static async Task<IResult> CreateAppointmentAsync(
        AppointmentDtoRequest dto,
        AppointmentService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.CreatingStart(dto);

        var appointment = await service.CreateAsync(dto);

        logger.Created(appointment.Id);
        return GetCreatedResult(appointment);
    }
    
    public static async Task<IResult> GetAppointmentAsync(
        int id,
        bool? services,
        AppointmentService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var appointment = await service.GetByIdAsync(id, new(services is true));
        return Results.Ok(appointment);
    }
    
    public static async Task<IResult> GetAllAppointmentsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        AppointmentService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(page, pageSize);

        var appointments = await service.GetAllAsync(page, pageSize);
        return Results.Ok(appointments);
    }

    public static async Task<IResult> UpdateAppointmentAsync(
        int id,
        AppointmentDtoRequest dto,
        AppointmentService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        await service.UpdateAsync(dto, id);

        logger.Updated(id);
        return Results.NoContent();
    }
    
    public static async Task<IResult> UpdatePaymentTypeAsync(
        int id,
        AppointmentPaymentTypeDtoUpdateRequest dto,
        AppointmentService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingPaymentStart(id, dto);

        await service.UpdatePaymentTypeAsync(dto, id);

        logger.UpdatedPayment(id);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointmentAsync(
        int id,
        AppointmentService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(id);

        await service.DeleteAsync(id);

        logger.Deleted(id);
        return Results.NoContent();
    }
}
