﻿using Microsoft.EntityFrameworkCore;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Application.Validators;
using ICorteApi.Presentation.Exceptions;
// using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Base;
using ICorteApi.Application.Interfaces;

namespace ICorteApi.Presentation.Endpoints;

public static class AddressEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.Address;
    private static readonly string ENDPOINT_NAME = EndpointNames.Address;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        // group.MapGet(INDEX, GetAddresses);
        group.MapGet("{id}", GetAddress);
        group.MapPost(INDEX, CreateAddress);
        group.MapPut("{id}", UpdateAddress);
        group.MapDelete("{id}", DeleteAddress);
    }
    
    public static async Task<IResult> GetAddress(int barberShopId, int id, AppDbContext context)
    {
        var address = await context.Addresses.SingleOrDefaultAsync(a => a.Id == id);

        if (address is null)
            return Results.NotFound("Agendamento não encontrado");

        var addressDto = address.CreateDto();
        return Results.Ok(addressDto);
    }

    public static async Task<IResult> CreateAddress(
        int barberShopId,
        AddressDtoRequest dto,
        IValidator<AddressDtoRequest> validator,
        IAddressService addressService)
    {
        var validationResult = validator.Validate(dto);
        
        if (!validationResult.IsValid)
            throw new AddressValidationException(
                validationResult.Errors
                    .Select(e => new Error(e.PropertyName, e.ErrorMessage))
                    .ToArray());
            // return Results.BadRequest(validationResult.Errors);

        var response = await addressService.CreateAsync(dto with { BarberShopId = barberShopId });

        if (!response.IsSuccess)
            return Results.BadRequest(Error.BadSave);
            
        string uri = $"/{ENDPOINT_PREFIX}/{response.Value!.Id}";
        return Results.Created(uri, new { Message = "Endereço criado com sucesso" });
    }

    public static async Task<IResult> UpdateAddress(
        int barberShopId,
        int id,
        AddressDtoRequest dto,
        AppDbContext context)
    {
        try
        {
            var address = await context.Addresses.SingleOrDefaultAsync(a => a.Id == id);

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

            await context.SaveChangesAsync();
            return Results.Created($"/address/{address.Id}", new { Message = "Endereço atualizado com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteAddress(int barberShopId, int id, AppDbContext context)
    {
        try
        {
            var address = await context.Addresses.SingleOrDefaultAsync(a => a.Id == id);

            if (address is null)
                return Results.NotFound();

            address.UpdatedAt = DateTime.UtcNow;
            address.IsDeleted = true;

            await context.SaveChangesAsync();
            return Results.Created($"/address/{address.Id}", new { Message = "Endereço removido com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
