﻿using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Enums;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class AddressEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.Address;
    private static readonly string ENDPOINT_NAME = EndpointNames.Address;

    public static IEndpointRouteBuilder MapAddressEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapPost(INDEX, CreateAddress);
        group.MapGet("{id}", GetAddress);
        group.MapPut("{id}", UpdateAddress);
        group.MapDelete("{id}", DeleteAddress);

        return app;
    }

    public static IResult GetCreatedResult(int newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.Address + "/" + newId;
        object value = new { Message = "Endereço criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateAddress(
        int barberShopId,
        AddressDtoCreate dto,
        IAddressService service)
    {
        var address = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(address.Id, address.BarberShopId);
    }

    public static async Task<IResult> GetAddress(
        int barberShopId,
        int id,
        IAddressService service)
    {
        var address = await service.GetByIdAsync(id, barberShopId);
        return Results.Ok(address);
    }

    public static async Task<IResult> UpdateAddress(
        int barberShopId,
        int id,
        AddressDtoUpdate dto,
        IAddressService service)
    {
        await service.UpdateAsync(dto, id, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAddress(
        int barberShopId,
        int id,
        IAddressService service)
    {
        await service.DeleteAsync(id, barberShopId);
        return Results.NoContent();
    }
}
