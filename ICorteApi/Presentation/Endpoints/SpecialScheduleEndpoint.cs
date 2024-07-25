using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class SpecialScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.SpecialSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.SpecialSchedule;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllSpecialSchedules);
        group.MapGet("{date}", GetSpecialSchedule);
        group.MapPost(INDEX, CreateSpecialSchedule);
        group.MapPut("{date}", UpdateSpecialSchedule);
        group.MapDelete("{date}", DeleteSpecialSchedule);
    }

    public static async Task<IResult> GetSpecialSchedule(
        int barberShopId,
        DateOnly date,
        ISpecialScheduleService specialScheduleService)
    {
        var response = await specialScheduleService.GetByIdAsync(date, barberShopId);

        if (!response.IsSuccess)
            return Results.NotFound();

        var specialScheduleDto = response.Value!.CreateDto();
        return Results.Ok(specialScheduleDto);
    }

    public static async Task<IResult> GetAllSpecialSchedules(
        int barberShopId,
        int page, int pageSize,
        ISpecialScheduleService specialScheduleService)
    {
        var response = await specialScheduleService.GetAllAsync(page, pageSize);

        if (!response.IsSuccess)
            return Results.BadRequest(response.Error);

        if (!response.Values.Any())
            return Results.NotFound();

        var dtos = response.Values
            .Select(dto => dto.CreateDto())
            .ToArray();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateSpecialSchedule(
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        ISpecialScheduleService specialScheduleService)
    {
        var response = await specialScheduleService.CreateAsync(dto);

        if (!response.IsSuccess)
            Results.BadRequest(response.Error);

        string uri = $"/{ENDPOINT_PREFIX}/{barberShopId}-{dto.Date}";
        return Results.Created(uri, new { Message = "Horário de funcionamento criado com sucesso" });
    }

    public static async Task<IResult> UpdateSpecialSchedule(
        int barberShopId,
        DateOnly date,
        SpecialScheduleDtoRequest dto,
        ISpecialScheduleService specialScheduleService)
    {
        var response = await specialScheduleService.UpdateAsync(date, barberShopId, dto);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSpecialSchedule(
        int barberShopId,
        DateOnly date,
        ISpecialScheduleService specialScheduleService)
    {
        var resp = await specialScheduleService.DeleteAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return Results.NotFound(resp.Error);

        return Results.NoContent();
    }
}
