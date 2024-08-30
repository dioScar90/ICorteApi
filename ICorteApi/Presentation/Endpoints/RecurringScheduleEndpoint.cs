using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class RecurringScheduleEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.RecurringSchedule;
    private static readonly string ENDPOINT_NAME = EndpointNames.RecurringSchedule;

    public static IEndpointRouteBuilder MapRecurringScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost(INDEX, CreateRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{dayOfWeek}", GetRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllRecurringSchedules)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{dayOfWeek}", UpdateRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
        group.MapDelete("{dayOfWeek}", DeleteRecurringSchedule)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    public static IResult GetCreatedResult(DayOfWeek newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.RecurringSchedule + "/" + newId;
        object value = new { Message = "Horário de funcionamento criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateRecurringSchedule(
        int barberShopId,
        RecurringScheduleDtoRequest dto,
        IValidator<RecurringScheduleDtoRequest> validator,
        IRecurringScheduleService service,
        IRecurringScheduleErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var schedule = await service.CreateAsync(dto, barberShopId);

        if (schedule is null)
            errors.ThrowCreateException();

        return GetCreatedResult(schedule!.DayOfWeek, schedule.BarberShopId);
    }

    public static async Task<IResult> GetRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService service,
        IRecurringScheduleErrors errors)
    {
        var schedule = await service.GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            errors.ThrowNotFoundException();
            
        return Results.Ok(schedule!.CreateDto());
    }

    public static async Task<IResult> GetAllRecurringSchedules(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IRecurringScheduleService service,
        IRecurringScheduleErrors errors)
    {
        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        
        var dtos = schedules?.Select(s => s.CreateDto()).ToArray() ?? [];
        return Results.Ok(dtos);
    }

    public static async Task<IResult> UpdateRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoRequest dto,
        IValidator<RecurringScheduleDtoRequest> validator,
        IRecurringScheduleService service,
        IRecurringScheduleErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var result = await service.UpdateAsync(dto, dayOfWeek, barberShopId);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteRecurringSchedule(
        int barberShopId,
        DayOfWeek dayOfWeek,
        IRecurringScheduleService service,
        IRecurringScheduleErrors errors)
    {
        var result = await service.DeleteAsync(dayOfWeek, barberShopId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
