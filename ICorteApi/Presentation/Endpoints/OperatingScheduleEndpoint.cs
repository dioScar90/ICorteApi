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
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllOperatingSchedules);
        group.MapGet("{barberShopId}", GetOperatingSchedule);
        group.MapPost("{barberShopId}", CreateOperatingSchedule);
        group.MapPut("{barberShopId}", UpdateOperatingSchedule);
        group.MapDelete("{barberShopId}", DeleteOperatingSchedule);
    }
    
    public static async Task<IResult> GetOperatingSchedule(
        int barberShopId,
        [FromQuery] DayOfWeek dayOfWeek,
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
        [FromQuery] DayOfWeek dayOfWeek,
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
        [FromQuery] DayOfWeek dayOfWeek,
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
