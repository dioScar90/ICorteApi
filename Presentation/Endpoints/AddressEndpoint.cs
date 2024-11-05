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

    public static IResult GetCreatedResult(AddressDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/address/{dto.Id}", new { Message = "Endereço criado com sucesso", Item = dto });

    public static async Task<IResult> CreateAddressAsync(
        int barberShopId,
        AddressDtoCreate dto,
        IAddressService service)
    {
        var address = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(address);
    }

    public static async Task<IResult> GetAddressAsync(
        int barberShopId,
        int id,
        IAddressService service)
    {
        var address = await service.GetByIdAsync(id, barberShopId);
        return Results.Ok(address);
    }

    public static async Task<IResult> UpdateAddressAsync(
        int barberShopId,
        int id,
        AddressDtoUpdate dto,
        IAddressService service)
    {
        await service.UpdateAsync(dto, id, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAddressAsync(
        int barberShopId,
        int id,
        IAddressService service)
    {
        await service.DeleteAsync(id, barberShopId);
        return Results.NoContent();
    }
}
