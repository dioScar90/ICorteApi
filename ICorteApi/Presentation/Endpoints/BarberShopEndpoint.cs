﻿using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;
using ICorteApi.Presentation.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop;
    private static readonly string ENDPOINT_NAME = EndpointNames.BarberShop;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllBarberShops);
        group.MapGet("{id}", GetBarberShop);
        group.MapPost(INDEX, CreateBarberShop);
        group.MapPut("{id}", UpdateBarberShop);
        group.MapDelete("{id}", DeleteBarberShop);
    }

    public static async Task<IResult> GetBarberShop(int id, IBarberShopService barberShopService)
    {
        var response = await barberShopService.GetByIdAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound();

        var barberShopDto = response.Value!.CreateDto();
        return Results.Ok(barberShopDto);
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

    public static async Task<IResult> GetAllBarberShops(
        int page,
        int pageSize,
        IBarberShopService barberShopService)
    {
        var response = await barberShopService.GetAllAsync(page, pageSize);

        if (!response.IsSuccess)
            return Results.BadRequest(response.Error);

        // if (!response.Data.Any())
        //     return Results.NotFound();

        var dtos = response.Values!
            .Select(b => b.CreateDto())
            .ToList();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateBarberShop(
        BarberShopDtoRequest dto,
        IBarberShopService barberShopService,
        IPersonService personService,
        IUserService userService)
    {
        var ownerId = await userService.GetUserIdAsync();

        if (ownerId is null)
            return Results.Unauthorized();

        var respPerson = await personService.GetByIdAsync((int)ownerId);

        if (!respPerson.IsSuccess)
            return Results.BadRequest();

        var newBarberShop = dto.CreateEntity()!;
        newBarberShop.OwnerId = respPerson.Value!.UserId;

        var response = await barberShopService.CreateAsync(respPerson.Value!.UserId, dto);

        if (!response.IsSuccess)
            Results.BadRequest(response.Error);

        string uri = $"/{ENDPOINT_PREFIX}/{newBarberShop!.Id}";
        return Results.Created(uri, new { Message = "Barbearia criada com sucesso" });
    }

    public static async Task<IResult> UpdateBarberShop(int id, BarberShopDtoRequest dto, IBarberShopService barberShopService)
    {
        var response = await barberShopService.UpdateAsync(dto, id);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteBarberShop(int id, IBarberShopService barberShopService)
    {
        var response = await barberShopService.DeleteAsync(id);

        if (!response.IsSuccess)
            return Results.BadRequest(response.Error);

        return Results.NoContent();
    }
}
