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
            // var barberShop = dto.CreateEntity<BarberShop>();
            var resposne = await barberShopService.UpdateAsync(id, dto);
            // var barberShop = await context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);

            // if (barberShop is null)
            //     return Results.NotFound();

            // barberShop.Name = dto.Name;
            // barberShop.Description = dto.Description;
            // barberShop.PhoneNumber = dto.PhoneNumber;
            // barberShop.ComercialNumber = dto.ComercialNumber;
            // barberShop.ComercialEmail = dto.ComercialEmail;
            // barberShop.OpeningHours = dto.OpeningHours;
            // barberShop.ClosingHours = dto.ClosingHours;
            // barberShop.DaysOpen = dto.DaysOpen;

            // if (dto.Address is not null)
            // {
            //     barberShop.Address.Street = dto.Address.Street;
            //     barberShop.Address.Number = dto.Address.Number;
            //     barberShop.Address.Complement = dto.Address.Complement;
            //     barberShop.Address.Neighborhood = dto.Address.Neighborhood;
            //     barberShop.Address.City = dto.Address.City;
            //     barberShop.Address.State = dto.Address.State;
            //     barberShop.Address.PostalCode = dto.Address.PostalCode;
            //     barberShop.Address.Country = dto.Address.Country;
            // }

            // barberShop.UpdatedAt = DateTime.UtcNow;

            // await context.SaveChangesAsync();
            // return Results.Ok(new { Message = "Barbearia atualizada com sucesso" });
            return Results.Ok(oie);
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
