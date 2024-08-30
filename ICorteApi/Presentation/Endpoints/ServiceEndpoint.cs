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

        group.MapPost(INDEX, CreateService)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{id}", GetService)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllServices)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

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

    public static async Task<IResult> CreateService(
        int barberShopId,
        ServiceDtoRequest dto,
        IValidator<ServiceDtoRequest> validator,
        IServiceService service,
        IServiceErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var serviceEntity = await service.CreateAsync(dto, barberShopId);

        if (serviceEntity is null)
            errors.ThrowCreateException();

        return GetCreatedResult(serviceEntity!.Id, serviceEntity.BarberShopId);
    }

    public static async Task<IResult> GetService(
        int barberShopId,
        int id,
        IServiceService service,
        IServiceErrors errors)
    {
        var serviceEntity = await service.GetByIdAsync(id, barberShopId);

        if (serviceEntity is null)
            errors.ThrowNotFoundException();
            
        return Results.Ok(serviceEntity!.CreateDto());
    }

    public static async Task<IResult> GetAllServices(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IServiceService service,
        IServiceErrors errors)
    {
        var services = await service.GetAllAsync(page, pageSize, barberShopId);
        
        var dtos = services?.Select(c => c.CreateDto()).ToArray() ?? [];
        return Results.Ok(dtos);
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
        var result = await service.UpdateAsync(dto, id, barberShopId);

        if (!result)
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
        var result = await service.DeleteAsync(id, barberShopId, forceDelete is true);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
