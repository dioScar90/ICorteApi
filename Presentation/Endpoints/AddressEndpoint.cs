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
    
    private static IResult GetCreatedResult(AddressDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/address/{dto.Id}", new { Message = "Endereço criado com sucesso", Item = dto });

    public static async Task<IResult> CreateAddressAsync(
        int barberShopId,
        AddressDtoRequest dto,
        AddressService service)
    {
        dto = dto with { BarberShopId = barberShopId };
        var address = await service.CreateAsync(dto);
        return GetCreatedResult(address);
    }

    public static async Task<IResult> GetAddressAsync(
        int barberShopId,
        int id,
        AddressService service)
    {
        var address = await service.GetByIdAsync(id, barberShopId);
        return Results.Ok(address);
    }

    public static async Task<IResult> UpdateAddressAsync(
        int barberShopId,
        int id,
        AddressDtoRequest dto,
        AddressService service)
    {
        dto = dto with { BarberShopId = barberShopId };
        await service.UpdateAsync(dto, id);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAddressAsync(
        int barberShopId,
        int id,
        AddressService service)
    {
        await service.DeleteAsync(id, barberShopId);
        return Results.NoContent();
    }
}
