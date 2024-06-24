using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICorteApi.Entities;
using ICorteApi.Context;
using ICorteApi.Dtos;
using ICorteApi.Extensions;

namespace ICorteApi.Routes;

public static class AddressesEndpoint
{
    public static void MapAddressesEndpoint(this IEndpointRouteBuilder app)
    {
        const string INDEX = "";
        var group = app.MapGroup("address");

        // group.MapGet(INDEX, GetAddresses);
        group.MapGet("{id}", GetAddress);
        group.MapPost(INDEX, CreateAddress);
        group.MapPut("{id}", UpdateAddress);
        group.MapDelete("{id}", DeleteAddress);
    }
    
    public static async Task<IResult> GetAddress(int id, ICorteContext context)
    {
        var appointment = await context.Addresses
            .SingleOrDefaultAsync(a => a.IsActive && a.Id == id);

        if (appointment is null)
            return Results.NotFound("Agendamento não encontrado");

        return Results.Ok(appointment);
    }
    
    public static async Task<IResult> CreateAddress(AddressDtoRequest dto, ICorteContext context)
    {
        try
        {
            Address newAddress = dto.CreateEntity<Address>()!;
            
            await context.Addresses.AddAsync(newAddress);
            int id = await context.SaveChangesAsync();

            return Results.Created($"/address/{id}", new { Message = "Endereço criado com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateAddress(int id, AddressDtoRequest dto, ICorteContext context)
    {
        try
        {
            if (id != dto.Id)
                return Results.BadRequest();

            var address = await context.Addresses.SingleOrDefaultAsync(a => a.IsActive && a.Id == id);

            if (address is null)
                return Results.NotFound();
            
            address.Street = dto.Street;
            address.Number = dto.Number;
            address.Complement = dto.Complement;
            address.Neighborhood = dto.Neighborhood;
            address.City = dto.City;
            address.State = dto.State;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;
            
            address.UpdatedAt = DateTime.UtcNow;

            id = await context.SaveChangesAsync();
            return Results.Created($"/address/{id}", new { Message = "Endereço atualizado com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteAddress(int id, ICorteContext context)
    {
        try
        {
            var address = await context.Addresses.SingleOrDefaultAsync(a => a.IsActive && a.Id == id);

            if (address is null)
                return Results.NotFound();
            
            address.UpdatedAt = DateTime.UtcNow;
            address.IsActive = false;

            id = await context.SaveChangesAsync();
            return Results.Created($"/address/{id}", new { Message = "Endereço removido com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
