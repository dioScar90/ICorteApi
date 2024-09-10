using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Enums;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class SpecialScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.SpecialSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.SpecialSchedule;

    public static IEndpointRouteBuilder MapSpecialScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost(INDEX, CreateSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{date}", GetSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllSpecialSchedules)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{date}", UpdateSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{date}", DeleteSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(DateOnly newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.SpecialSchedule + "/" + newId;
        object value = new { Message = "Horário especial criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateSpecialSchedule(
        int barberShopId,
        SpecialScheduleDtoCreate dto,
        ISpecialScheduleService service)
    {
        var schedule = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(schedule.Date, schedule.BarberShopId);
    }

    public static async Task<IResult> GetSpecialSchedule(
        DateOnly date,
        int barberShopId,
        ISpecialScheduleService service)
    {
        var schedule = await service.GetByIdAsync(date, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllSpecialSchedules(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ISpecialScheduleService service)
    {
        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateSpecialSchedule(
        DateOnly date,
        int barberShopId,
        SpecialScheduleDtoUpdate dto,
        ISpecialScheduleService service)
    {
        await service.UpdateAsync(dto, date, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSpecialSchedule(
        DateOnly date,
        int barberShopId,
        ISpecialScheduleService service)
    {
        await service.DeleteAsync(date, barberShopId);
        return Results.NoContent();
    }
}
