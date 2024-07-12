using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class OperatingScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.OperatingSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.OperatingSchedule;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllOperatingSchedules);
        group.MapGet("{dayOfWeek}", GetOperatingSchedule);
        group.MapPost(INDEX, CreateOperatingSchedule);
        group.MapPut("{dayOfWeek}", UpdateOperatingSchedule);
        group.MapDelete("{dayOfWeek}", DeleteOperatingSchedule);
    }

    public static async Task<IResult> GetOperatingSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var response = await operatingScheduleService.GetByIdAsync(dayOfWeek, barberShopId);

            if (!response.Success)
                return Results.NotFound();

            var operatingScheduleDto = response.Data!.CreateDto<OperatingScheduleDtoResponse>();
            return Results.Ok(operatingScheduleDto);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> GetAllOperatingSchedules(
        int barberShopId,
        IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var response = await operatingScheduleService.GetAllAsync(barberShopId);

            if (!response.Success)
                return Results.BadRequest(response.Message);

            if (!response.Data.Any())
                return Results.NotFound();

            var dtos = response.Data
                .Select(b => b.CreateDto<OperatingScheduleDtoResponse>())
                .ToList();

            return Results.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CreateOperatingSchedule(
        int barberShopId,
        OperatingScheduleDtoRequest dto,
        IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var response = await operatingScheduleService.CreateAsync(barberShopId, dto);

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

    public static async Task<IResult> UpdateOperatingSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        OperatingScheduleDtoRequest dto,
        IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var response = await operatingScheduleService.UpdateAsync(barberShopId, dto);

            if (!response.Success)
                return Results.NotFound(response);

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteOperatingSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var resp = await operatingScheduleService.DeleteAsync(dayOfWeek, barberShopId);

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
