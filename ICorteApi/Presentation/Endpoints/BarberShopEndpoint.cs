namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop;
    private static readonly string ENDPOINT_NAME = EndpointNames.BarberShop;

    public static IEndpointRouteBuilder MapBarberShopEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);
        
        group.MapPost(INDEX, CreateBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
        
        group.MapPut("{id}", UpdateBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        group.MapDelete("{id}", DeleteBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        return app;
    }
    
    public static IResult GetCreatedResult(int newId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + newId;
        object value = new { Message = "Barbearia criada com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> CreateBarberShop(
        BarberShopDtoCreate dto,
        IBarberShopService service,
        IUserService userService)
    {
        int ownerId = await userService.GetMyUserIdAsync();
        var barberShop = await service.CreateAsync(dto, ownerId);
        return GetCreatedResult(barberShop.Id);
    }

    public static async Task<IResult> GetBarberShop(
        int id,
        IBarberShopService service)
    {
        var barberShop = await service.GetByIdAsync(id);
        return Results.Ok(barberShop);
    }

    public static async Task<IResult> UpdateBarberShop(
        int id,
        BarberShopDtoUpdate dto,
        IBarberShopService service,
        IUserService userService)
    {
        int ownerId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, ownerId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteBarberShop(
        int id,
        IBarberShopService service,
        IUserService userService)
    {
        int ownerId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, ownerId);
        return Results.NoContent();
    }
}
