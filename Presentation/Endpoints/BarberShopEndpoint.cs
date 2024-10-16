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
        
        group.MapPut("{id}", UpdateBarberShopAsync)
            .WithSummary("Update BarberShop")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        group.MapDelete("{id}", DeleteBarberShopAsync)
            .WithSummary("Delete BarberShop")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        return app;
    }
    
    public static IResult GetCreatedResult(int newId) =>
        Results.Created($"barber-shop/{newId}", new { Message = "Barbearia criada com sucesso" });
    
    public static async Task<IResult> CreateBarberShopAsync(
        BarberShopDtoCreate dto,
        IBarberShopService service,
        IUserService userService)
    {
        int ownerId = await userService.GetMyUserIdAsync();
        var barberShop = await service.CreateAsync(dto, ownerId);
        return GetCreatedResult(barberShop.Id);
    }

    public static async Task<IResult> GetBarberShopAsync(
        int id,
        IBarberShopService service)
    {
        var barberShop = await service.GetByIdAsync(id);
        return Results.Ok(barberShop);
    }

    public static async Task<IResult> UpdateBarberShopAsync(
        int id,
        BarberShopDtoUpdate dto,
        IBarberShopService service,
        IUserService userService)
    {
        int ownerId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, ownerId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteBarberShopAsync(
        int id,
        IBarberShopService service,
        IUserService userService)
    {
        int ownerId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, ownerId);
        return Results.NoContent();
    }
}
