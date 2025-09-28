using ICorteApi.Application.Services;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    public static IEndpointRouteBuilder MapBarberShopEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop").WithTags("Barber Shop");
        
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
    
    private static IResult GetCreatedResult(BarberShopDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.Id}", new { Message = "Barbearia criada com sucesso", Item = dto });
    
    public static async Task<IResult> CreateBarberShopAsync(
        BarberShopDtoRequest dto,
        BarberShopService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.CreatingStart(dto);

        var barberShop = await service.CreateAsync(dto);

        logger.Created(barberShop.Id);
        return GetCreatedResult(barberShop);
    }
    
    public static async Task<IResult> GetBarberShopAsync(
        int id,
        BarberShopService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var barberShop = await service.GetByIdAsync(id);
        return Results.Ok(barberShop);
    }
    
    public static async Task<IResult> GetAppointmentsByBarberShopAsync(
        int barberShopId,
        int? page,
        int? pageSize,
        BarberShopService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAppointmentsByBarbershopStart(barberShopId, page, pageSize);

        var barberShop = await service.GetAppointmentsByBarberShopAsync(barberShopId, page ?? 1, pageSize ?? 25);
        return Results.Ok(barberShop);
    }

    public static async Task<IResult> UpdateBarberShopAsync(
        int id,
        BarberShopDtoRequest dto,
        BarberShopService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        var appointments = await service.UpdateAsync(dto, id);

        logger.Updated(id);
        return Results.Ok(appointments);
    }

    public static async Task<IResult> DeleteBarberShopAsync(
        int id,
        BarberShopService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(id);

        await service.DeleteAsync(id);

        logger.Deleted(id);
        return Results.NoContent();
    }
}
