using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.BarberSchedule;

    public static IEndpointRouteBuilder MapBarberScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapGet("{barberShopId}/dates/{dateOfWeek}", GetAvailableDatesForBarberAsync)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{barberShopId}/slots/{date}", GetAvailableSlotsAsync)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("top-barbers/{dateOfWeek}", GetTopBarbersWithAvailabilityAsync)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }

    public static async Task<IResult> GetAvailableDatesForBarberAsync(
        int barberShopId,
        DateOnly dateOfWeek,
        IBarberScheduleService service)
    {
        var barberShop = await service.GetAvailableDatesForBarberAsync(barberShopId, dateOfWeek);
        return Results.Ok(barberShop);
    }

    public static async Task<IResult> GetAvailableSlotsAsync(
        int barberShopId,
        DateOnly date,
        [FromQuery] int[] serviceIds,
        IBarberScheduleService service)
    {
        var times = await service.GetAvailableSlotsAsync(barberShopId, date, serviceIds);
        return Results.Ok(times);
    }

    public static async Task<IResult> GetTopBarbersWithAvailabilityAsync(
        DateOnly dateOfWeek,
        [FromQuery] int? take,
        IBarberScheduleService service)
    {
        var barberShops = await service.GetTopBarbersWithAvailabilityAsync(dateOfWeek, take);
        return Results.Ok(barberShops);
    }
}
