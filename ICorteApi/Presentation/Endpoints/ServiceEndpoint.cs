using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ServiceEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.Service;
    private static readonly string ENDPOINT_NAME = EndpointNames.Service;

    public static IEndpointRouteBuilder MapServiceEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapGet("{id}", GetService)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllServices)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPost(INDEX, CreateService)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapPut("{id}", UpdateService)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{id}", DeleteService)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
        return app;
    }
    
    public static IResult GetCreatedResult(int newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.Service + "/" + newId;
        object value = new { Message = "Serviço criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetService(
        int barberShopId,
        int id,
        IServiceService service,
        IServiceErrors errors)
    {
        var res = await service.GetByIdAsync(id, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        var address = res.Value!;
        
        var addressDto = address.CreateDto();
        return Results.Ok(addressDto);
    }

    public static async Task<IResult> GetAllServices(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IServiceService service,
        IServiceErrors errors)
    {
        var res = await service.GetAllAsync(page, pageSize, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowBadRequestException(res.Error);
            
        var dtos = res.Values!
            .Select(c => c.CreateDto())
            .ToList();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateService(
        int barberShopId,
        ServiceDtoRequest dto,
        IValidator<ServiceDtoRequest> validator,
        IServiceService service,
        IServiceErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.CreateAsync(dto, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(response.Value!.Id, barberShopId);
    }

    public static async Task<IResult> UpdateService(
        int barberShopId,
        int id,
        ServiceDtoRequest dto,
        IValidator<ServiceDtoRequest> validator,
        IServiceService service,
        IServiceErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.UpdateAsync(dto, id, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteService(
        bool? forceDelete,
        int barberShopId,
        int id,
        IServiceService service,
        IServiceErrors errors)
    {
        var response = await service.DeleteAsync(id, barberShopId, forceDelete is true);

        if (!response.IsSuccess)
            errors.ThrowDeleteException(response.Error);

        return Results.NoContent();
    }
}
