using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICorteApi.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using ICorteApi.Entities;
using ICorteApi.Repositories;
using ICorteApi.Dtos;
using ICorteApi.Extensions;

namespace ICorteApi.Routes;

public static class BarbersEndpoint
{
    public static void MapBarbersEndpoint(this IEndpointRouteBuilder app)
    {
        const string INDEX = "";
        var group = app.MapGroup("barber");

        group.MapGet(INDEX, GetAllBarbers);
        group.MapGet("{id}", GetBarber);
        group.MapPost(INDEX, CreateBarber);
        group.MapPut("{id}", UpdateBarber);
        group.MapDelete("{id}", DeleteBarber);
    }

    public static async Task<IResult> GetBarber(int id, ICorteContext context)
    {
        var barber = await context.Barbers
            .Include(b => b.Address)
            .SingleOrDefaultAsync(b => b.IsActive && b.Id == id);

        if (barber is null)
            return Results.NotFound();

        var dto = BarberToDtoResponse(barber);
        return Results.Ok(dto);
    }

    public static async Task<Results<Ok<List<BarberDtoResponse>>, NotFound<string>>> GetAllBarbers(int page, int perPage, ICorteContext context)
    {
        var barbers = new BarberRepository(context)
            .GetAll(page, perPage);

        if (!barbers.Any())
            TypedResults.NotFound();

        var dtos = await barbers
            .Select(b => BarberToDtoResponse(b))
            .ToListAsync();
            
        return TypedResults.Ok(dtos);
    }
    
    public static async Task<IResult> CreateBarber(BarberDtoRequest dto, ICorteContext context)
    {
        try
        {
            var newBarber = dto.CreateEntity<Barber>();

            await context.Barbers.AddAsync(newBarber!);

            var id = await context.SaveChangesAsync();

            return Results.Created($"/barber/{id}", new { Message = "Barbeiro criado com sucesso" });
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
    
    
    public static async Task<Results<Ok<string>, NotFound, BadRequest<string>>> UpdateBarber(
        int id,
        [FromBody] BarberDtoRequest dto,
        ICorteContext context)
    {
        try
        {
            var barber = await context.Barbers.SingleOrDefaultAsync(b => b.IsActive && b.Id == id);

            if (barber is null)
                return TypedResults.NotFound();
            
            barber.Name = dto.Name;
            
            barber.Address.StreetType = dto.Address.StreetType;
            barber.Address.Street = dto.Address.Street;
            barber.Address.Number = dto.Address.Number;
            barber.Address.Complement = dto.Address.Complement;
            barber.Address.Neighborhood = dto.Address.Neighborhood;
            barber.Address.City = dto.Address.City;
            barber.Address.State = dto.Address.State;
            barber.Address.PostalCode = dto.Address.PostalCode;
            barber.Address.Country = dto.Address.Country;

            barber.UpdatedAt = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            
            return TypedResults.Ok("Barbeiro atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
    
    public static async Task<Results<NoContent, NotFound, BadRequest<string>>> DeleteBarber(int id, ICorteContext context)
    {
        try
        {
            var barber = await context.Barbers.SingleOrDefaultAsync(b => b.IsActive && b.Id == id);

            if (barber is null)
                return TypedResults.NotFound();

            context.Barbers.Remove(barber);
            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static BarberDtoResponse BarberToDtoResponse(Barber barber) =>
        new(
            barber.Id,
            barber.Name,
            new(
                barber.Address.Id,
                barber.Address.StreetType,
                barber.Address.Street,
                barber.Address.Number,
                barber.Address.Complement,
                barber.Address.Neighborhood,
                barber.Address.City,
                barber.Address.State,
                barber.Address.PostalCode,
                barber.Address.Country
            )
        );
}
