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
        try
        {
            var response = await recurringScheduleService.GetByIdAsync(dayOfWeek, barberShopId);

            if (!response.Success)
                return Results.NotFound();

            var recurringScheduleDto = response.Data!.CreateDto<RecurringScheduleDtoResponse>();
            return Results.Ok(recurringScheduleDto);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> GetAllRecurringSchedules(
        int barberShopId,
        IRecurringScheduleService recurringScheduleService)
    {
        try
        {
            var response = await recurringScheduleService.GetAllAsync(barberShopId);

            if (!response.Success)
                return Results.BadRequest(response.Message);

            if (!response.Data.Any())
                return Results.NotFound();

            var dtos = response.Data
                .Select(b => b.CreateDto<RecurringScheduleDtoResponse>())
                .ToList();

            return Results.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CreateRecurringSchedule(
        int barberShopId,
        RecurringScheduleDtoRequest dto,
        IRecurringScheduleService recurringScheduleService)
    {
        try
        {
            var response = await recurringScheduleService.CreateAsync(barberShopId, dto);

            if (!response.Success)
                Results.BadRequest(response);

            string uri = $"/{ENDPOINT_PREFIX}/{barberShopId}-{dto.DayOfWeek}";
            return Results.Created(uri, new { Message = "Horário de funcionamento criado com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoRequest dto,
        IRecurringScheduleService recurringScheduleService)
    {
        try
        {
            var response = await recurringScheduleService.UpdateAsync(barberShopId, dto);

            if (!response.Success)
                return Results.NotFound(response);

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService recurringScheduleService)
    {
        try
        {
            var resp = await recurringScheduleService.DeleteAsync(dayOfWeek, barberShopId);

            if (!resp.Success)
                return Results.NotFound(resp);

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
