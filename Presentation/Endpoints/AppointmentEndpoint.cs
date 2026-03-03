using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class AppointmentEndpoint
{
    private static string GetBaseEndpoint(AppointmentDtoResponse? appointment = null) => appointment is null
        ? "appointment"
        : $"appointment/{appointment.Id}";

    public static IEndpointRouteBuilder MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Appointment");

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
    
    public static async Task<Results<Created<AppointmentDtoResponse>, UnprocessableEntity<Error>, Conflict<Error>, BadRequest<Error>>> CreateAppointmentAsync(
        AppointmentDtoRequest dto,
        AppointmentService service,
        AppointmentErrors errors,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        if (dto.Services.Length == 0)
            return errors.EmptyServices();
            
        if (!await serviceService.IsServicesFromUniqueBarberShop(dto.Services))
            return errors.NotBarberShopIdsUniqueFromServices();
            
        logger.CreatingStart(dto);
        
        var appointment = await service.CreateAsync(dto);
        
        if (appointment is null)
            return errors.Create();
            
        logger.Created(appointment.Id);
        return TypedResults.Created(GetBaseEndpoint(appointment), appointment);
    }
    
    public static async Task<Results<Ok<AppointmentDtoResponse>, NotFound<Error>, Conflict<Error>>> GetAppointmentAsync(
        int id,
        bool? services,
        AppointmentService service,
        AppointmentErrors errors,
        UserService userService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.GettingStart(id);

        var appointment = await service.GetByIdAsync(id, new(services is true));

        if (appointment is null)
            return errors.NotFound();
            
        if (!await service.AppointmentBelongsToClientAsync(id))
            return errors.AppointmentNotBelongsToClient();
        
        return TypedResults.Ok(appointment);
    }
    
    public static async Task<Ok<PaginationResponse<AppointmentDtoResponse>>> GetAllAppointmentsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        AppointmentService service,
        AppointmentErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.GettingAllStart(page, pageSize);

        var appointments = await service.GetAllAsync(page, pageSize);
        return TypedResults.Ok(appointments);
    }

    public static async Task<Results<NoContent, UnprocessableEntity<Error>, BadRequest<Error>, Conflict<Error>>> UpdateAppointmentAsync(
        int id,
        AppointmentDtoRequest dto,
        AppointmentService service,
        AppointmentErrors errors,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.UpdatingStart(id, dto);
        
        if (!await serviceService.IsServicesFromUniqueBarberShop(dto.Services))
            return errors.NotBarberShopIdsUniqueFromServices();
            
        if (!await service.AppointmentBelongsToClientAsync(id))
            return errors.AppointmentNotBelongsToClient();
            
        if (!await service.UpdateAsync(dto, id))
            return errors.Update();

        logger.Updated(id);
        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, BadRequest<Error>, Conflict<Error>>> UpdatePaymentTypeAsync(
        int id,
        AppointmentPaymentTypeDtoUpdateRequest dto,
        AppointmentService service,
        AppointmentErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.UpdatingPaymentStart(id, dto);
            
        if (!await service.AppointmentBelongsToClientAsync(id))
            return errors.AppointmentNotBelongsToClient();

        if (!await service.UpdatePaymentTypeAsync(dto, id))
            return errors.Update();

        logger.UpdatedPayment(id);
        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NoContent, BadRequest<Error>, Conflict<Error>>> DeleteAppointmentAsync(
        int id,
        AppointmentService service,
        AppointmentErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.DeletingStart(id);
            
        if (!await service.AppointmentBelongsToClientAsync(id))
            return errors.AppointmentNotBelongsToClient();

        if (!await service.DeleteAsync(id))
            return errors.Delete();
            
        logger.Deleted(id);
        return TypedResults.NoContent();
    }
}
