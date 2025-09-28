using ICorteApi.Application.Services;

namespace ICorteApi.Presentation.Endpoints;

public static class AddressEndpoint
{
    public static IEndpointRouteBuilder MapAddressEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/address").WithTags("Address");

        group.MapPost("", CreateAddressAsync)
            .WithSummary("Create Address")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{id}", GetAddressAsync)
            .WithSummary("Get Address")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapPut("{id}", UpdateAddressAsync)
            .WithSummary("Update Address")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{id}", DeleteAddressAsync)
            .WithSummary("Delete Address")
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
                Entity = nameof(Address),
                Logger = loggerFactory.CreateLogger(nameof(AddressEndpoint))
            };
        }
        
        internal void CreatingStart(AddressDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);

        internal void UpdatingStart(int id, AddressDtoRequest dto) =>
            Logger.LogInformation("Received request to update {Entity} with Id={Id} {@Entity}", Entity, id, dto);

        internal void Updated(int id) =>
            Logger.LogInformation("{Entity} successfully updated with Id={Id}", Entity, id);

        internal void DeletingStart(int id) =>
            Logger.LogInformation("Received request to delete {Entity} Id={Id}", Entity, id);

        internal void Deleted(int id) =>
            Logger.LogInformation("{Entity} successfully deleted with Id={Id}", Entity, id);
    }
    
    private static IResult GetCreatedResult(AddressDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/address/{dto.Id}", new { Message = "Endereço criado com sucesso", Item = dto });

    public static async Task<IResult> CreateAddressAsync(
        int barberShopId,
        AddressDtoRequest dto,
        AddressService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.CreatingStart(dto);

        dto = dto with { BarberShopId = barberShopId };
        var address = await service.CreateAsync(dto);
        
        logger.Created(address.Id);
        return GetCreatedResult(address);
    }

    public static async Task<IResult> GetAddressAsync(
        int barberShopId,
        int id,
        AddressService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var address = await service.GetByIdAsync(id, barberShopId);
        return Results.Ok(address);
    }

    public static async Task<IResult> UpdateAddressAsync(
        int barberShopId,
        int id,
        AddressDtoRequest dto,
        AddressService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        dto = dto with { BarberShopId = barberShopId };
        await service.UpdateAsync(dto, id);
        
        logger.Updated(id);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAddressAsync(
        int barberShopId,
        int id,
        AddressService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(id);

        await service.DeleteAsync(id, barberShopId);
        
        logger.Deleted(id);
        return Results.NoContent();
    }
}
