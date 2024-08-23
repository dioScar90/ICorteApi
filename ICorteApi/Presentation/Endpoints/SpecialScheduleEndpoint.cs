using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class SpecialScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.SpecialSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.SpecialSchedule;

    public static IEndpointRouteBuilder MapSpecialScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapGet(INDEX, GetAllSpecialSchedules)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{date}", GetSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPost(INDEX, CreateSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapPut("{date}", UpdateSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{date}", DeleteSpecialSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(DateOnly newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.SpecialSchedule + "/" + newId;
        object value = new { Message = "Horário especial criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetSpecialSchedule(
        int barberShopId,
        DateOnly date,
        ISpecialScheduleService service,
        ISpecialScheduleErrors errors)
    {
        var res = await service.GetByIdAsync(date, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException(res.Error);
            
        var scheduleDto = res.Value!.CreateDto();
        return Results.Ok(scheduleDto);
    }

    public static async Task<IResult> GetAllSpecialSchedules(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ISpecialScheduleService service,
        ISpecialScheduleErrors errors)
    {
        var response = await service.GetAllAsync(page, pageSize, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowNotFoundException();

        var dtos = response.Values!
            .Select(b => b.CreateDto())
            .ToArray();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateSpecialSchedule(
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        IValidator<SpecialScheduleDtoRequest> validator,
        ISpecialScheduleService service,
        ISpecialScheduleErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.CreateAsync(dto, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(response.Value!.Date, barberShopId);
    }

    public static async Task<IResult> UpdateSpecialSchedule(
        int barberShopId,
        DateOnly date,
        SpecialScheduleDtoRequest dto,
        IValidator<SpecialScheduleDtoRequest> validator,
        ISpecialScheduleService service,
        ISpecialScheduleErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.UpdateAsync(dto, date, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSpecialSchedule(
        int barberShopId,
        DateOnly date,
        ISpecialScheduleService service,
        ISpecialScheduleErrors errors)
    {
        var response = await service.DeleteAsync(date, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
