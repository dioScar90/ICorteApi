using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ICorteApi.Presentation.Endpoints;

public static class AddressEndpoint
{
    private static string GetBaseEndpoint(AddressDtoResponse? address = null) => address is null
        ? "barber-shop/{barberShopId}/address"
        : $"barber-shop/{address.BarberShopId}/address/{address.Id}";

    public static IEndpointRouteBuilder MapAddressEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Address");

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
    
    public static async Task<Results<Created<AddressDtoResponse>, BadRequest<Error>>> CreateAddressAsync(
        int barberShopId,
        AddressDtoRequest dto,
        AddressService service,
        AddressErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.CreatingStart(dto);
        
        dto = dto with { BarberShopId = barberShopId };
        var address = await service.CreateAsync(dto, cancellationToken);

        if (address is null)
            return errors.Create();
        
        logger.Created(address.Id);
        return TypedResults.Created(GetBaseEndpoint(address), address);
    }
    
    public static async Task<Results<Ok<AddressDtoResponse>, NotFound<Error>, Conflict<Error>>> GetAddressAsync(
        int id,
        AddressService service,
        AddressErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);
        logger.GettingStart(id);
        
        var address = await service.GetByIdAsync(id, cancellationToken);
        
        if (address is null)
            return errors.NotFound();
            
        return TypedResults.Ok(address);
    }
    
    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> UpdateAddressAsync(
        int id,
        AddressDtoRequest dto,
        AddressService service,
        AddressErrors errors,
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
            return errors.AddressBelongsToAnotherBarberShop();
            
        if (!await service.UpdateAsync(dto, id, cancellationToken))
            return errors.Update();
        
        logger.Updated(id);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> DeleteAddressAsync(
        int id,
        AddressService service,
        AddressErrors errors,
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
            return errors.AddressBelongsToAnotherBarberShop();
        
        if (!await service.DeleteAsync(id, cancellationToken))
            return errors.Delete();
        
        logger.Deleted(id);
        return TypedResults.NoContent();
    }
}
