using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Enums;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class RecurringScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.RecurringSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.RecurringSchedule;

    public static IEndpointRouteBuilder MapRecurringScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost(INDEX, CreateRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{dayOfWeek}", GetRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllRecurringSchedules)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{dayOfWeek}", UpdateRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
        group.MapDelete("{dayOfWeek}", DeleteRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(DayOfWeek newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.RecurringSchedule + "/" + newId;
        object value = new { Message = "Horário de funcionamento criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateRecurringSchedule(
        int barberShopId,
        RecurringScheduleDtoCreate dto,
        IRecurringScheduleService service)
    {
        var schedule = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(schedule.DayOfWeek, schedule.BarberShopId);
    }

    public static async Task<IResult> GetRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService service)
    {
        var schedule = await service.GetByIdAsync(dayOfWeek, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllRecurringSchedules(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IRecurringScheduleService service)
    {
        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoUpdate dto,
        IRecurringScheduleService service)
    {
        await service.UpdateAsync(dto, dayOfWeek, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService service)
    {
        await service.DeleteAsync(dayOfWeek, barberShopId);
        return Results.NoContent();
    }
}
