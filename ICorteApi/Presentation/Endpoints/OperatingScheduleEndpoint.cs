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

    public static void MapOperatingScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            // .WithGroupName(ENDPOINT_NAME)
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
        int barberShopId,
        // [FromQuery(Name = "page")] int pageAux,
        // [FromQuery(Name = "pageSize")] int pageSizeAux,
        IOperatingScheduleService operatingScheduleService)
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
        OperatingScheduleDtoRequest dto,
        IPersonService personService,
        IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var responseBarberShop = await personService.GetMyBarberShopAsync();

            if (!responseBarberShop.Success)
                return Results.NotFound(responseBarberShop);
            
            int barberShopId = responseBarberShop.Data.Id;
            var response = await operatingScheduleService.CreateAsync(barberShopId, dto);

            if (!response.Success)
                Results.BadRequest(response.Message);

            DayOfWeek dayOfWeek = response.Data.DayOfWeek;

            return Results.Created($"/{ENDPOINT_PREFIX}/{barberShopId}-{dayOfWeek}", new { Message = "Barbearia criada com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateOperatingSchedule(int id, OperatingScheduleDtoRequest dto, IOperatingScheduleService operatingScheduleService)
    {
        try
        {
            var response = await operatingScheduleService.UpdateAsync(id, dto);

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
