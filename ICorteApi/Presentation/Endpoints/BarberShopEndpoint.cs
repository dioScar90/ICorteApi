using Microsoft.EntityFrameworkCore;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Application.Dtos;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    public static void MapBarberShopEndpoint(this IEndpointRouteBuilder app)
    {
        const string INDEX = "";
        var group = app.MapGroup("barberShop");

        group.MapGet(INDEX, GetAllBarbers);
        group.MapGet("{id}", GetBarberShop);
        group.MapPost(INDEX, CreateBarberShop);
        group.MapPut("{id}", UpdateBarberShop);
        group.MapDelete("{id}", DeleteBarberShop);
    }

    public static async Task<IResult> GetBarberShop(int id, IBarberShopService barberShopService)
    {
        try
        {
            var response = await barberShopService.GetByIdAsync(id);

            if (!response.Success)
                return Results.NotFound();

            var barberShopDto = response.Data.CreateDto<BarberShopDtoResponse>();
            return Results.Ok(barberShopDto);
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

    public static async Task<IResult> GetAllBarbers(
        [FromQuery(Name = "page")] int pageAux,
        [FromQuery(Name = "pageSize")] int pageSizeAux,
        IBarberShopService barberShopService)
    {
        try
        {
            var (page, pageSize) = SanitizeIndexAndPageSize(pageAux, pageSizeAux);
            var response = await barberShopService.GetAllAsync(page, pageSize);

            if (!response.Success)
                return Results.BadRequest(response.Message);

            if (!response.Data.Any())
                return Results.NotFound();
            
            var dtos = response.Data
                .Select(b => b.CreateDto<BarberShopDtoResponse>())
                .ToList();

            return Results.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CreateBarberShop(BarberShopDtoRequest dto, IBarberShopService barberShopService)
    {
        try
        {
            var newBarberShop = dto.CreateEntity<BarberShop>();
            var response = await barberShopService.CreateAsync(newBarberShop!);

            if (!response.Success)
                Results.BadRequest(response.Message);

            return Results.Created($"/barber/{newBarberShop!.Id}", new { Message = "Barbearia criada com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateBarberShop(int id, BarberShopDtoRequest dto, IBarberShopService barberShopService)
    {
        try
        {
            var response = await barberShopService.UpdateAsync(id, dto);

            if (!response.Success)
                return Results.NotFound(response);
            
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteBarberShop(int id, AppDbContext context)
    {
        try
        {
            var barberShop = await context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);

            if (barberShop is null)
                return Results.NotFound();

            barberShop.UpdatedAt = DateTime.UtcNow;
            barberShop.IsDeleted = true;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
