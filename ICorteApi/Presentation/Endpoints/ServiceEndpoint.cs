using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Enums;
using ICorteApi.Application.Interfaces;
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
        ServiceDtoCreate dto,
        IServiceService service)
    {
        var serviceEntity = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(serviceEntity.Id, serviceEntity.BarberShopId);
    }

    public static async Task<IResult> GetService(
        int id,
        int barberShopId,
        IServiceService service)
    {
        var serviceDto = await service.GetByIdAsync(id, barberShopId);
        return Results.Ok(serviceDto);
    }

    public static async Task<IResult> GetAllServices(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IServiceService service)
    {
        var services = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(services);
    }

    public static async Task<IResult> UpdateService(
        int id,
        int barberShopId,
        ServiceDtoUpdate dto,
        IServiceService service)
    {
        await service.UpdateAsync(dto, id, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteService(
        [FromQuery] bool? forceDelete,
        int id,
        int barberShopId,
        IServiceService service)
    {
        await service.DeleteAsync(id, barberShopId, forceDelete is true);
        return Results.NoContent();
    }
}
