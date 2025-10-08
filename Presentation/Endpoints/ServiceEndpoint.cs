using ICorteApi.Application.Services;
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

        group.MapGet("{serviceId}", GetServiceAsync)
            .WithSummary("Get Service")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllServicesAsync)
            .WithSummary("Get All Services")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{serviceId}", UpdateServiceAsync)
            .WithSummary("Update Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{serviceId}", DeleteServiceAsync)
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
    
    public static async Task<Created<ServiceDtoResponse>> CreateServiceAsync(
        int barberShopId,
        ServiceDtoRequest dto,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var service = await serviceService.CreateAsync(dto);

        logger.Created(service.Id);
        return TypedResults.Created(GetBaseEndpoint(service), service);
    }

    public static async Task<Ok<ServiceDtoResponse>> GetServiceAsync(
        int serviceId,
        int barberShopId,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(serviceId);

        var service = await serviceService.GetByIdAsync(serviceId, barberShopId);
        return TypedResults.Ok(service);
    }

    public static async Task<Ok<PaginationResponse<ServiceDtoResponse>>> GetAllServicesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var services = await serviceService.GetAllAsync(page, pageSize, barberShopId);
        return TypedResults.Ok(services);
    }

    public static async Task<NoContent> UpdateServiceAsync(
        int serviceId,
        int barberShopId,
        ServiceDtoRequest dto,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(serviceId, dto);

        await serviceService.UpdateAsync(dto, serviceId, barberShopId);

        logger.Updated(serviceId);
        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteServiceAsync(
        [FromQuery] bool? forceDelete,
        int serviceId,
        int barberShopId,
        ServiceService serviceService,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(serviceId);

        await serviceService.DeleteAsync(serviceId, barberShopId, forceDelete is true);

        logger.Deleted(serviceId);
        return TypedResults.NoContent();
    }
}
