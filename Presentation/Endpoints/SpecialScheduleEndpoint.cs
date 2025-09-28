using ICorteApi.Application.Services;
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
    
    private static IResult GetCreatedResult(SpecialScheduleDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/special-schedule/{dto.Date}", new { Message = "Horário especial criado com sucesso", Item = dto });

    public static async Task<IResult> CreateSpecialScheduleAsync(
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        SpecialScheduleService service)
    {
        dto = dto with { BarberShopId = barberShopId };
        var schedule = await service.CreateAsync(dto);
        return GetCreatedResult(schedule);
    }

    public static async Task<IResult> GetSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service)
    {
        var schedule = await service.GetByIdAsync(date, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllSpecialSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        SpecialScheduleService service)
    {
        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        SpecialScheduleService service)
    {
        dto = dto with { BarberShopId = barberShopId };
        await service.UpdateAsync(dto, date, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service)
    {
        await service.DeleteAsync(date, barberShopId);
        return Results.NoContent();
    }
}
