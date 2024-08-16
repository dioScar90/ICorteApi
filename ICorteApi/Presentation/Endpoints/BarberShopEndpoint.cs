﻿using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop;
    private static readonly string ENDPOINT_NAME = EndpointNames.BarberShop;

    public static IEndpointRouteBuilder MapBarberShopEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);
        
        group.MapPost(INDEX, CreateBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        group.MapPut("{id}", UpdateBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        group.MapDelete("{id}", DeleteBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        return app;
    }
    
    public static IResult GetCreatedResult(int newId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + newId;
        object value = new { Message = "Barbearia criada com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetBarberShop(
        int id,
        IBarberShopService service,
        IBarberShopErrors errors)
    {
        var response = await service.GetByIdAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound();

        var barberShopDto = response.Value!.CreateDto();
        return Results.Ok(barberShopDto);
    }
    
    public static async Task<IResult> CreateBarberShop(
        BarberShopDtoRequest dto,
        IValidator<BarberShopDtoRequest> validator,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int ownerId = userService.GetMyUserId();
        var response = await service.CreateAsync(dto, ownerId);

        if (!response.IsSuccess)
            errors.ThrowCreateException();
        
        return GetCreatedResult(response.Value!.Id);
    }

    public static async Task<IResult> UpdateBarberShop(
        int id,
        BarberShopDtoRequest dto,
        IValidator<BarberShopDtoRequest> validator,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        
        var response = await service.UpdateAsync(dto, id);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteBarberShop(
        int id,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        var response = await service.DeleteAsync(id);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();
        
        return Results.NoContent();
    }
}
