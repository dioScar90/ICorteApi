using Microsoft.EntityFrameworkCore;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Application.Dtos;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class OperatingScheduleEndpoint
{
    private const string INDEX = "";
    private const string ENDPOINT_PREFIX = "operating-schedule";
    private const string ENDPOINT_NAME = "Operating Schedule";

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            // .WithGroupName(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet("{barberShopId}", GetAllOperatingSchedules);
        group.MapGet("{barberShopId}-{dayOfWeek}", GetOperatingSchedule);
        group.MapPost("{barberShopId}", CreateOperatingSchedule);
        group.MapPut("{barberShopId}", UpdateOperatingSchedule);
        group.MapDelete("{barberShopId}-{dayOfWeek}", DeleteOperatingSchedule);
    }
    
    public static async Task<IResult> GetOperatingSchedule(
        [FromRoute] int barberShopId,
        [FromRoute] DayOfWeek dayOfWeek,
        [FromServices] IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var response = await operatingScheduleService.GetByIdAsync(dayOfWeek, barberShopId);

            if (!response.Success)
                return Results.NotFound();

            var operatingScheduleDto = response.Data.CreateDto<OperatingScheduleDtoResponse>();
            return Results.Ok(operatingScheduleDto);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static (int, int) SanitizeIndexAndPageSize(int page, int? pageSize)
    {
        const int DEFAULT_PAGE_SIZE = 25;

        if (page < 1)
            page = 1;

        pageSize ??= DEFAULT_PAGE_SIZE;

        if (pageSize < 1)
            pageSize = DEFAULT_PAGE_SIZE;

        return (page, (int)pageSize);
    }

    public static async Task<IResult> GetAllOperatingSchedules(
        [FromRoute] int barberShopId,
        // [FromQuery(Name = "page")] int pageAux,
        // [FromQuery(Name = "pageSize")] int pageSizeAux,
        [FromServices] IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            // var (page, pageSize) = SanitizeIndexAndPageSize(pageAux, pageSizeAux);
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
        [FromRoute] int barberShopId,
        [FromBody] OperatingScheduleDtoRequest dto,
        // IPersonService personService,
        [FromServices] IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            // var responseBarberShop = await personService.GetMyBarberShopAsync();

            // if (!responseBarberShop.Success)
            //     return Results.NotFound(responseBarberShop);
            
            // int barberShopId = responseBarberShop.Data.Id;
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
        [FromRoute] int barberShopId,
        [FromBody] OperatingScheduleDtoRequest dto,
        [FromServices] IOperatingScheduleService operatingScheduleService)
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
        [FromRoute] int barberShopId,
        [FromRoute] DayOfWeek dayOfWeek,
        [FromServices] IOperatingScheduleService operatingScheduleService)
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
