using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class RecurringScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.RecurringSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.RecurringSchedule;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllRecurringSchedules);
        group.MapGet("{dayOfWeek}", GetRecurringSchedule);
        group.MapPost(INDEX, CreateRecurringSchedule);
        group.MapPut("{dayOfWeek}", UpdateRecurringSchedule);
        group.MapDelete("{dayOfWeek}", DeleteRecurringSchedule);
    }

    public static async Task<IResult> GetRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService recurringScheduleService)
    {
        var response = await recurringScheduleService.GetByIdAsync(dayOfWeek, barberShopId);

        if (!response.IsSuccess)
            return Results.NotFound();

        var recurringScheduleDto = response.Value!.CreateDto();
        return Results.Ok(recurringScheduleDto);
    }

    public static async Task<IResult> GetAllRecurringSchedules(
        int barberShopId,
        int page, int pageSize,
        IRecurringScheduleService recurringScheduleService)
    {
        var response = await recurringScheduleService.GetAllAsync(page, pageSize);

        if (!response.IsSuccess)
            return Results.BadRequest(response.Error);

        if (!response.Values.Any())
            return Results.NotFound();

        var dtos = response.Values
            .Select(dto => dto.CreateDto())
            .ToArray();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateRecurringSchedule(
        int barberShopId,
        RecurringScheduleDtoRequest dto,
        IRecurringScheduleService recurringScheduleService)
    {
        var response = await recurringScheduleService.CreateAsync(dto with { BarberShopId = barberShopId });

        if (!response.IsSuccess)
            Results.BadRequest(response.Error);

        string uri = $"/{ENDPOINT_PREFIX}/{barberShopId}-{dto.DayOfWeek}";
        return Results.Created(uri, new { Message = "Horário de funcionamento criado com sucesso" });
    }

    public static async Task<IResult> UpdateRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoRequest dto,
        IRecurringScheduleService recurringScheduleService)
    {
        var response = await recurringScheduleService.UpdateAsync(dayOfWeek, barberShopId, dto);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService recurringScheduleService)
    {
        var resp = await recurringScheduleService.DeleteAsync(dayOfWeek, barberShopId);

        if (!resp.IsSuccess)
            return Results.NotFound(resp.Error);

        return Results.NoContent();
    }
}
