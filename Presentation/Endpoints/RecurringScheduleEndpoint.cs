using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class RecurringScheduleEndpoint
{
    public static IEndpointRouteBuilder MapRecurringScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/recurring-schedule").WithTags("Recurring Schedule");

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
    
    private static IResult GetCreatedResult(RecurringScheduleDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/recurring-dto/{dto.DayOfWeek}", new { Message = "Horário de funcionamento criado com sucesso", Item = dto });

    public static async Task<IResult> CreateRecurringScheduleAsync(
        int barberShopId,
        RecurringScheduleDtoRequest dto,
        RecurringScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        
        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var schedule = await service.CreateAsync(dto);

        logger.Created(schedule.DayOfWeek, schedule.BarberShopId);
        return GetCreatedResult(schedule);
    }

    public static async Task<IResult> GetRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(dayOfWeek, barberShopId);

        var schedule = await service.GetByIdAsync(dayOfWeek, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllRecurringSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        RecurringScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleDtoRequest dto,
        RecurringScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.UpdatingStart(dayOfWeek, barberShopId, dto);

        await service.UpdateAsync(dto, dayOfWeek, barberShopId);

        logger.Updated(dayOfWeek, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteRecurringScheduleAsync(
        int barberShopId,
        DayOfWeek dayOfWeek,
        RecurringScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(dayOfWeek, barberShopId);

        await service.DeleteAsync(dayOfWeek, barberShopId);

        logger.Deleted(dayOfWeek, barberShopId);
        return Results.NoContent();
    }
}
