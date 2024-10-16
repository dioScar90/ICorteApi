using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class SpecialScheduleEndpoint
{
    public static IEndpointRouteBuilder MapSpecialScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/special-schedule").WithTags("Special Schedule");

        group.MapPost("", CreateSpecialScheduleAsync)
            .WithSummary("Create Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{date}", GetSpecialScheduleAsync)
            .WithSummary("Get Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllSpecialSchedulesAsync)
            .WithSummary("Get All Special Schedules")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{date}", UpdateSpecialScheduleAsync)
            .WithSummary("Update Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{date}", DeleteSpecialScheduleAsync)
            .WithSummary("Delete Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(DateOnly newId, int barberShopId) =>
        Results.Created($"barber-shop/{barberShopId}/special-schedule/{newId}", new { Message = "Horário especial criado com sucesso" });

    public static async Task<IResult> CreateSpecialScheduleAsync(
        int barberShopId,
        SpecialScheduleDtoCreate dto,
        ISpecialScheduleService service)
    {
        var schedule = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(schedule.Date, schedule.BarberShopId);
    }

    public static async Task<IResult> GetSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        ISpecialScheduleService service)
    {
        var schedule = await service.GetByIdAsync(date, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllSpecialSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ISpecialScheduleService service)
    {
        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleDtoUpdate dto,
        ISpecialScheduleService service)
    {
        await service.UpdateAsync(dto, date, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        ISpecialScheduleService service)
    {
        await service.DeleteAsync(date, barberShopId);
        return Results.NoContent();
    }
}
