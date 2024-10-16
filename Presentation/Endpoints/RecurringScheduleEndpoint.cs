using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class RecurringScheduleEndpoint
{
    public static IEndpointRouteBuilder MapRecurringScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/recurring-schedule").WithTags("Recurring Schedule");

        group.MapPost("", CreateRecurringScheduleAsync)
            .WithSummary("Create Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{dayOfWeek}", GetRecurringScheduleAsync)
            .WithSummary("Get Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllRecurringSchedulesAsync)
            .WithSummary("Get All Recurring Schedules")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{dayOfWeek}", UpdateRecurringScheduleAsync)
            .WithSummary("Update Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
        group.MapDelete("{dayOfWeek}", DeleteRecurringScheduleAsync)
            .WithSummary("Delete Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(DayOfWeek newId, int barberShopId) =>
        Results.Created($"barber-shop/{barberShopId}/recurring-schedule/{newId}", new { Message = "Horário de funcionamento criado com sucesso" });

    public static async Task<IResult> CreateRecurringScheduleAsync(
        int barberShopId,
        RecurringScheduleDtoCreate dto,
        IRecurringScheduleService service)
    {
        var schedule = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(schedule.DayOfWeek, schedule.BarberShopId);
    }

    public static async Task<IResult> GetRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService service)
    {
        var schedule = await service.GetByIdAsync(dayOfWeek, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllRecurringSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IRecurringScheduleService service)
    {
        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoUpdate dto,
        IRecurringScheduleService service)
    {
        await service.UpdateAsync(dto, dayOfWeek, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService service)
    {
        await service.DeleteAsync(dayOfWeek, barberShopId);
        return Results.NoContent();
    }
}
