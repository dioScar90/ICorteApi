using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICorteApi.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using ICorteApi.Entities;
using ICorteApi.Dtos;

namespace ICorteApi.Routes;

public static class BarbersEndpoint
{
    public static void MapBarbersEndpoint(this IEndpointRouteBuilder app)
    {
        const string INDEX = "";
        var group = app.MapGroup("barber");

        group.MapGet("{id}", GetBarber);
        group.MapPost(INDEX, CreateBarber);
        group.MapPut("{id}", UpdateBarber);
        group.MapDelete("{id}", DeleteBarber);
    }

    public static async Task<Results<Ok<Barber>, NotFound<string>>> GetBarber(int id, ICorteContext context)
    {
        var barber = await context.Barbers
            .SingleOrDefaultAsync(b => b.IsActive && b.Id == id);

        if (barber is null)
            TypedResults.NotFound("Não encontrado");

        return TypedResults.Ok(barber);
    }
    
    public static async Task<Results<Created, BadRequest<string>>> CreateBarber([FromBody] BarberDto dto, ICorteContext context)
    {
        try
        {
            Barber newBarber = new()
            {
                Name = dto.Name,
                Address = new()
                {
                    StreetType = dto.Address.StreetType,
                    Street = dto.Address.Street,
                    Number = dto.Address.Number,
                    Complement = dto.Address.Complement,
                    Neighborhood = dto.Address.Neighborhood,
                    City = dto.Address.City,
                    State = dto.Address.State,
                    PostalCode = dto.Address.PostalCode,
                    Country = dto.Address.Country,
                }
            };

            await context.Barbers.AddAsync(newBarber);
            var id = await context.SaveChangesAsync();

            return TypedResults.Created("Sua mãe é minha");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
    
    
    public static async Task<Results<Ok<string>, NotFound, BadRequest<string>>> UpdateBarber(
        int id,
        [FromBody] BarberDto dto,
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
}
