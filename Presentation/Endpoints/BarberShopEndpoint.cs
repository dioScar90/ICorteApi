using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    private static string GetBaseEndpoint(BarberShopDtoResponse? barberShop = null) => barberShop is null
        ? "barber-shop"
        : $"barber-shop/{barberShop.Id}";
        
    public static IEndpointRouteBuilder MapBarberShopEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Barber Shop");

        group.MapPost("", CreateBarberShopAsync)
            .WithSummary("Create BarberShop")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetBarberShopAsync)
            .WithSummary("Get BarberShop")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{barberShopId}/appointments", GetAppointmentsByBarberShopAsync)
            .WithSummary("Get Appointments By BarberShop")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapPut("{id}", UpdateBarberShopAsync)
            .WithSummary("Update BarberShop")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{id}", DeleteBarberShopAsync)
            .WithSummary("Delete BarberShop")
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
                Entity = nameof(BarberShop),
                Logger = loggerFactory.CreateLogger(nameof(BarberShopEndpoint))
            };
        }

        internal void CreatingStart(BarberShopDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);

        internal void GettingAppointmentsByBarbershopStart(int barberShopId, int? page, int? pageSize) =>
            Logger.LogInformation(
                "Received request to get appointments by barbershop {Entity} with BarberShopId={BarberShopId} Page={Page} PageSize={PageSize}",
                Entity, barberShopId, page, pageSize);

        internal void UpdatingStart(int id, BarberShopDtoRequest dto) =>
            Logger.LogInformation("Received request to update {Entity} with Id={Id} {@Entity}", Entity, id, dto);

        internal void Updated(int id) =>
            Logger.LogInformation("{Entity} successfully updated with Id={Id}", Entity, id);
            
        internal void DeletingStart(int id) =>
            Logger.LogInformation("Received request to delete {Entity} Id={Id}", Entity, id);

        internal void Deleted(int id) =>
            Logger.LogInformation("{Entity} successfully deleted with Id={Id}", Entity, id);
    }
    
    public static async Task<Results<Created<BarberShopDtoResponse>, BadRequest<Error>>> CreateBarberShopAsync(
        BarberShopDtoRequest dto,
        BarberShopService service,
        BarberShopErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.CreatingStart(dto);

        var barberShop = await service.CreateAsync(dto, cancellationToken);
        
        if (barberShop is null)
            return errors.Create();
            
        logger.Created(barberShop!.Id);
        return TypedResults.Created(GetBaseEndpoint(barberShop), barberShop);
    }
    
    public static async Task<Results<Ok<BarberShopDtoResponse>, NotFound<Error>>> GetBarberShopAsync(
        int id,
        BarberShopService service,
        BarberShopErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var barberShop = await service.GetByIdAsync(id, false, false, cancellationToken);

        if (barberShop is null)
            return errors.NotFound();

        return TypedResults.Ok(barberShop);
    }
    
    public static async Task<Ok<PaginationResponse<AppointmentsByBarberShopDtoResponse>>> GetAppointmentsByBarberShopAsync(
        int barberShopId,
        int? page,
        int? pageSize,
        BarberShopService service,
        BarberShopErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.GettingAppointmentsByBarbershopStart(barberShopId, page, pageSize);

        var barberShop = await service.GetAppointmentsByBarberShopAsync(barberShopId, page ?? 1, pageSize ?? 25, cancellationToken);
        return TypedResults.Ok(barberShop);
    }
    
    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> UpdateBarberShopAsync(
        int id,
        BarberShopDtoRequest dto,
        BarberShopService service,
        BarberShopErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.UpdatingStart(id, dto);
        
        var infos = await service.GetInfosAsync(id, cancellationToken);
        
        if (!infos.Exists)
            return errors.NotFound();
            
        if (!infos.BelongsToMe)
            return errors.BarberShopBelongsToAnotherOwner();

        if (!await service.UpdateAsync(dto, id, cancellationToken))
            return errors.Update();

        logger.Updated(id);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> DeleteBarberShopAsync(
        int id,
        BarberShopService service,
        BarberShopErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.DeletingStart(id);

        var infos = await service.GetInfosAsync(id, cancellationToken);
        
        if (!infos.Exists)
            return errors.NotFound();
            
        if (!infos.BelongsToMe)
            return errors.BarberShopBelongsToAnotherOwner();

        if (!await service.DeleteAsync(id, cancellationToken))
            return errors.Delete();

        logger.Deleted(id);
        return TypedResults.NoContent();
    }
}
