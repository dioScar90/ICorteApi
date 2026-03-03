using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class RecurringScheduleEndpoint
{
    private static string GetBaseEndpoint(RecurringScheduleDtoResponse? schedule = null) => schedule is null
        ? "barber-shop/{barberShopId}/recurring-schedule"
        : $"barber-shop/{schedule.BarberShopId}/recurring-schedule/{schedule.DayOfWeek}";

    public static IEndpointRouteBuilder MapRecurringScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Recurring Schedule");

        group.MapPost("", CreateRecurringScheduleAsync)
            .WithSummary("Create Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{dayOfWeek}", GetRecurringScheduleAsync)
            .WithSummary("Get Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllRecurringSchedulesAsync)
            .WithSummary("Get All Recurring Schedules")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{dayOfWeek}", UpdateRecurringScheduleAsync)
            .WithSummary("Update Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
            
        group.MapDelete("{dayOfWeek}", DeleteRecurringScheduleAsync)
            .WithSummary("Delete Recurring Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        return app;
    }
    
    internal record LoggerActions
    {
        private string Entity;
        private ILogger Logger;

        private LoggerActions() { }

        internal static LoggerActions FactoryCreate(ILoggerFactory loggerFactory)
        {
            return new()
            {
                Entity = nameof(RecurringSchedule),
                Logger = loggerFactory.CreateLogger(nameof(RecurringScheduleEndpoint))
            };
        }

        internal void CreatingStart(RecurringScheduleDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(DayOfWeek dayOfWeek, int barberShopId) =>
            Logger.LogInformation(
                "{Entity} successfully created with DayOfWeek={DayOfWeek} BarberShopId={BarberShopId}",
                Entity, dayOfWeek, barberShopId);

        internal void GettingStart(DayOfWeek dayOfWeek, int barberShopId) =>
            Logger.LogInformation
            ("Received request to get {Entity} with DayOfWeek={DayOfWeek} BarberShopId={BarberShopId}",
                Entity, dayOfWeek, barberShopId);

        internal void GettingAllStart(int barberShopId, int? page, int? pageSize) =>
            Logger.LogInformation(
                "Received request to get all {Entity} with BarberShopId={BarberShopId} Page={Page} PageSize={PageSize}",
                Entity, barberShopId, page, pageSize);

        internal void UpdatingStart(DayOfWeek dayOfWeek, int barberShopId, RecurringScheduleDtoRequest dto) =>
            Logger.LogInformation
            ("Received request to update {Entity} with DayOfWeek={DayOfWeek} BarberShopId={BarberShopId} {@Entity}",
                Entity, dayOfWeek, barberShopId, dto);

        internal void Updated(DayOfWeek dayOfWeek, int barberShopId) =>
            Logger.LogInformation(
                "{Entity} successfully updated with DayOfWeek={DayOfWeek} BarberShopId={BarberShopId}",
                Entity, dayOfWeek, barberShopId);
            
        internal void DeletingStart(DayOfWeek dayOfWeek, int barberShopId) =>
            Logger.LogInformation
            ("Received request to delete {Entity} DayOfWeek={DayOfWeek} BarberShopId={BarberShopId}",
                Entity, dayOfWeek, barberShopId);

        internal void Deleted(DayOfWeek dayOfWeek, int barberShopId) =>
            Logger.LogInformation(
                "{Entity} successfully deleted with DayOfWeek={DayOfWeek} BarberShopId={BarberShopId}",
                Entity, dayOfWeek, barberShopId);
    }
    
    public static async Task<Results<Created<RecurringScheduleDtoResponse>, BadRequest<Error>>> CreateRecurringScheduleAsync(
        int barberShopId,
        RecurringScheduleDtoRequest dto,
        RecurringScheduleService service,
        RecurringScheduleErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        
        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var schedule = await service.CreateAsync(dto);

        if (schedule is null)
            return errors.Create();

        logger.Created(schedule.DayOfWeek, schedule.BarberShopId);
        return TypedResults.Created(GetBaseEndpoint(schedule), schedule);
    }

    public static async Task<Results<Ok<RecurringScheduleDtoResponse>, NotFound<Error>>> GetRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleService service,
        RecurringScheduleErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(dayOfWeek, barberShopId);

        var schedule = await service.GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            return errors.NotFound();

        return TypedResults.Ok(schedule);
    }

    public static async Task<Ok<PaginationResponse<RecurringScheduleDtoResponse>>> GetAllRecurringSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        RecurringScheduleService service,
        RecurringScheduleErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return TypedResults.Ok(schedules);
    }
    
    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> UpdateRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoRequest dto,
        RecurringScheduleService service,
        RecurringScheduleErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.UpdatingStart(dayOfWeek, barberShopId, dto);

        if (!await service.RecurringScheduleExists(dayOfWeek, barberShopId))
            return errors.NotFound();

        if (!await service.RecurringScheduleBelongsToBarberShop(dayOfWeek, barberShopId))
            return errors.RecurringScheduleNotBelongsToBarberShop();
            
        if (!await service.UpdateAsync(dto, dayOfWeek, barberShopId))
            return errors.Update();

        logger.Updated(dayOfWeek, barberShopId);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> DeleteRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleService service,
        RecurringScheduleErrors errors,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(dayOfWeek, barberShopId);

        if (!await service.RecurringScheduleExists(dayOfWeek, barberShopId))
            return errors.NotFound();

        if (!await service.RecurringScheduleBelongsToBarberShop(dayOfWeek, barberShopId))
            return errors.RecurringScheduleNotBelongsToBarberShop();

        if (!await service.DeleteAsync(dayOfWeek, barberShopId))
            return errors.Delete();

        logger.Deleted(dayOfWeek, barberShopId);
        return TypedResults.NoContent();
    }
}
