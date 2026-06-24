using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ServiceEndpoint
{
    private static string GetBaseEndpoint(ServiceDtoResponse? service = null) => service is null
        ? "barber-shop/{barberShopId}/service"
        : $"barber-shop/{service.BarberShopId}/service/{service.Id}";

    public static IEndpointRouteBuilder MapServiceEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Service");

        group.MapPost("", CreateServiceAsync)
            .WithSummary("Create Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{id}", GetServiceAsync)
            .WithSummary("Get Service")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllServicesAsync)
            .WithSummary("Get All Services")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{id}", UpdateServiceAsync)
            .WithSummary("Update Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{id}", DeleteServiceAsync)
            .WithSummary("Delete Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
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
                Entity = nameof(Service),
                Logger = loggerFactory.CreateLogger(nameof(ServiceEndpoint))
            };
        }

        internal void CreatingStart(ServiceDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);

        internal void GettingAllStart(int barberShopId, int? page, int? pageSize) =>
            Logger.LogInformation(
                "Received request to get all {Entity} with BarberShopId={BarberShopId} Page={Page} PageSize={PageSize}",
                Entity, barberShopId, page, pageSize);

        internal void UpdatingStart(int id, ServiceDtoRequest dto) =>
            Logger.LogInformation("Received request to update {Entity} with Id={Id} {@Entity}", Entity, id, dto);

        internal void Updated(int id) =>
            Logger.LogInformation("{Entity} successfully updated with Id={Id}", Entity, id);
            
        internal void DeletingStart(int id) =>
            Logger.LogInformation("Received request to delete {Entity} Id={Id}", Entity, id);

        internal void Deleted(int id) =>
            Logger.LogInformation("{Entity} successfully deleted with Id={Id}", Entity, id);
    }
    
    public static async Task<Results<Created<ServiceDtoResponse>, BadRequest<Error>>> CreateServiceAsync(
        int barberShopId,
        ServiceDtoRequest dto,
        ServiceService serviceService,
        ServiceErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var service = await serviceService.CreateAsync(dto, cancellationToken);

        if (service is null)
            return errors.Create();

        logger.Created(service.Id);
        return TypedResults.Created(GetBaseEndpoint(service), service);
    }

    public static async Task<Results<Ok<ServiceDtoResponse>, NotFound<Error>, Conflict<Error>>> GetServiceAsync(
        int id,
        int barberShopId,
        ServiceService serviceService,
        ServiceErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.GettingStart(id);
        
        var service = await serviceService.GetByIdAsync(id, false, cancellationToken);
        
        if (service is null)
            return errors.NotFound();
        
        if (service.BarberShopId != barberShopId)
            return errors.ServiceBelongsToAnotherBarberShop();
            
        return TypedResults.Ok(service);
    }

    public static async Task<Ok<PaginationResponse<ServiceDtoResponse>>> GetAllServicesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ServiceService serviceService,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var services = await serviceService.GetAllAsync(page, pageSize, barberShopId, cancellationToken);
        return TypedResults.Ok(services);
    }

    public static async Task<Results<Ok<ServiceDtoResponse>, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> UpdateServiceAsync(
        int id,
        int barberShopId,
        ServiceDtoRequest dto,
        ServiceService serviceService,
        ServiceErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        var infos = await serviceService.GetEntityInfosAsync(id, barberShopId, cancellationToken);
        
        if (!infos.Exists)
            return errors.NotFound();

        if (!infos.BelongsToCurrentUser)
            return errors.ServiceBelongsToAnotherBarberShop();

        if (!infos.BelongsToBarberShop)
            return errors.ServiceBelongsToAnotherBarberShop();
            
        var service = await serviceService.UpdateAsync(dto, id, cancellationToken);

        if (service is null)
            return errors.Update();

        logger.Updated(id);
        return TypedResults.Ok(service);
    }
    
    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> DeleteServiceAsync(
        [FromQuery] bool? forceDelete,
        int id,
        int barberShopId,
        ServiceService serviceService,
        ServiceErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.DeletingStart(id);

        var infos = await serviceService.GetEntityInfosAsync(id, barberShopId, cancellationToken);
        
        if (!infos.Exists)
            return errors.NotFound();

        if (!infos.BelongsToCurrentUser)
            return errors.ServiceBelongsToAnotherBarberShop();

        if (!infos.BelongsToBarberShop)
            return errors.ServiceBelongsToAnotherBarberShop();
            
        async Task<DateOnly[]> GetCorrelatedDates()
        {
            if (forceDelete is true)
                return [];
            
            if (!await serviceService.CheckCorrelatedAppointmentsAsync(id, cancellationToken))
                return [];

            return await serviceService.GetDatesFromCorrelatedAppointmentsAsync(id, cancellationToken);
        }
        
        var dates = await GetCorrelatedDates();
        
        if (dates.Length > 0)
            return errors.ThereAreStillAppointments(dates);
        
        if (!await serviceService.DeleteAsync(id, cancellationToken))
            return errors.Delete();

        logger.Deleted(id);
        return TypedResults.NoContent();
    }
}
