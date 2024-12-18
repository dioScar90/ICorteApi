﻿using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ServiceEndpoint
{
    public static IEndpointRouteBuilder MapServiceEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/service").WithTags("Service");

        group.MapPost("", CreateServiceAsync)
            .WithSummary("Create Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{serviceId}", GetServiceAsync)
            .WithSummary("Get Service")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllServicesAsync)
            .WithSummary("Get All Services")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{serviceId}", UpdateServiceAsync)
            .WithSummary("Update Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{serviceId}", DeleteServiceAsync)
            .WithSummary("Delete Service")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
        return app;
    }
    
    public static IResult GetCreatedResult(ServiceDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/service/{dto.Id}", new { Message = "Serviço criado com sucesso", Item = dto });

    public static async Task<IResult> CreateServiceAsync(
        int barberShopId,
        ServiceDtoCreate dto,
        IServiceService service)
    {
        var serviceEntity = await service.CreateAsync(dto, barberShopId);
        return GetCreatedResult(serviceEntity);
    }

    public static async Task<IResult> GetServiceAsync(
        int serviceId,
        int barberShopId,
        IServiceService service)
    {
        var serviceDto = await service.GetByIdAsync(serviceId, barberShopId);
        return Results.Ok(serviceDto);
    }

    public static async Task<IResult> GetAllServicesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IServiceService service)
    {
        var services = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(services);
    }

    public static async Task<IResult> UpdateServiceAsync(
        int serviceId,
        int barberShopId,
        ServiceDtoUpdate dto,
        IServiceService service)
    {
        await service.UpdateAsync(dto, serviceId, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteServiceAsync(
        [FromQuery] bool? forceDelete,
        int serviceId,
        int barberShopId,
        IServiceService service)
    {
        await service.DeleteAsync(serviceId, barberShopId, forceDelete is true);
        return Results.NoContent();
    }
}
