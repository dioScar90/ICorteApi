using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberScheduleEndpoint
{
    public static IEndpointRouteBuilder MapBarberScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-schedule").WithTags("Barber Schedule");

        group.MapGet("{barberShopId}/dates/{dateOfWeek}", GetAvailableDatesForBarberAsync)
            .WithSummary("Get Available Dates For Barber")
            .WithDescription("Get all available dates a barber has in a specific week by providing one of that week's date.")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{barberShopId}/slots/{date}", GetAvailableSlotsAsync)
            .WithSummary("Get Available Slots")
            .WithDescription("Get all possible slots of time a barber has to get a new appointment in a specific day by providing also the wishing services.")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("top-barbers/{dateOfWeek}", GetTopBarbersWithAvailabilityAsync)
            .WithSummary("Get Top Barbers With Availability")
            .WithDescription("Get the barbers with the highest rates with available times to get new appointments in a specific week by providing one of that week's date.")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("services", SearchServicesByNameAsync)
            .WithSummary("Search Services By Name")
            .WithDescription("Get the services that matches in total or in part of a given value to be searched.")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        return app;
    }

    public static async Task<IResult> GetAvailableDatesForBarberAsync(
        int barberShopId,
        DateOnly dateOfWeek,
        IBarberScheduleService service)
    {
        var dates = await service.GetAvailableDatesForBarberAsync(barberShopId, dateOfWeek);
        return Results.Ok(dates);
    }

    public static async Task<IResult> GetAvailableSlotsAsync(
        int barberShopId,
        DateOnly date,
        [FromQuery] int[] serviceIds,
        IBarberScheduleService service)
    {
        var slots = await service.GetAvailableSlotsAsync(barberShopId, date, serviceIds);
        return Results.Ok(slots);
    }

    public static async Task<IResult> GetTopBarbersWithAvailabilityAsync(
        DateOnly dateOfWeek,
        [FromQuery] int? take,
        IBarberScheduleService service)
    {
        var TopBarberShopDtoResponses = await service.GetTopBarbersWithAvailabilityAsync(dateOfWeek, take);
        return Results.Ok(TopBarberShopDtoResponses);
    }
    
    public static async Task<IResult> SearchServicesByNameAsync(
        [FromQuery] string q,
        IBarberScheduleService service)
    {
        var TopBarberShopDtoResponses = await service.SearchServicesByName(q);
        return Results.Ok(TopBarberShopDtoResponses);
    }
}
