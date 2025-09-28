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
    
    private static IResult GetCreatedResult(BarberShopDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.Id}", new { Message = "Barbearia criada com sucesso", Item = dto });
    
    public static async Task<IResult> CreateBarberShopAsync(
        BarberShopDtoRequest dto,
        BarberShopService service)
    {
        var barberShop = await service.CreateAsync(dto);
        return GetCreatedResult(barberShop);
    }
    
    public static async Task<IResult> GetBarberShopAsync(
        int id,
        BarberShopService service)
    {
        var barberShop = await service.GetByIdAsync(id);
        return Results.Ok(barberShop);
    }
    
    public static async Task<IResult> GetAppointmentsByBarberShopAsync(
        int barberShopId,
        int? page,
        int? pageSize,
        BarberShopService service)
    {
        var barberShop = await service.GetAppointmentsByBarberShopAsync(barberShopId, page ?? 1, pageSize ?? 25);
        return Results.Ok(barberShop);
    }

    public static async Task<IResult> UpdateBarberShopAsync(
        int id,
        BarberShopDtoRequest dto,
        BarberShopService service)
    {
        var appointments = await service.UpdateAsync(dto, id);
        return Results.Ok(appointments);
    }

    public static async Task<IResult> DeleteBarberShopAsync(
        int id,
        BarberShopService service)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}
