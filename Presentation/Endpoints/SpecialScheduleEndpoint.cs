using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class SpecialScheduleEndpoint
{
    private static string GetBaseEndpoint(SpecialScheduleDtoResponse? schedule = null) => schedule is null
        ? "barber-shop/{barberShopId}/special-schedule"
        : $"barber-shop/{schedule.BarberShopId}/special-schedule/{schedule.Date}";

    public static IEndpointRouteBuilder MapSpecialScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Special Schedule");

        group.MapPost("", CreateSpecialScheduleAsync)
            .WithSummary("Create Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapGet("{date}/{id}", GetSpecialScheduleAsync)
            .WithSummary("Get Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllSpecialSchedulesAsync)
            .WithSummary("Get All Special Schedules")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{date}/{id}", UpdateSpecialScheduleAsync)
            .WithSummary("Update Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{date}/{id}", DeleteSpecialScheduleAsync)
            .WithSummary("Delete Special Schedule")
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
                Entity = nameof(SpecialSchedule),
                Logger = loggerFactory.CreateLogger(nameof(SpecialScheduleEndpoint))
            };
        }

        internal void CreatingStart(SpecialScheduleDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(DateOnly date, int barberShopId) =>
            Logger.LogInformation(
                "{Entity} successfully created with Date={Date} BarberShopId={BarberShopId}",
                Entity, date, barberShopId);

        internal void GettingStart(DateOnly date, int barberShopId) =>
            Logger.LogInformation
            ("Received request to get {Entity} with Date={Date} BarberShopId={BarberShopId}",
                Entity, date, barberShopId);

        internal void GettingAllStart(int barberShopId, int? page, int? pageSize) =>
            Logger.LogInformation(
                "Received request to get all {Entity} with BarberShopId={BarberShopId} Page={Page} PageSize={PageSize}",
                Entity, barberShopId, page, pageSize);

        internal void UpdatingStart(DateOnly date, int barberShopId, SpecialScheduleDtoRequest dto) =>
            Logger.LogInformation
            ("Received request to update {Entity} with Date={Date} BarberShopId={BarberShopId} {@Entity}",
                Entity, date, barberShopId, dto);

        internal void Updated(DateOnly date, int barberShopId) =>
            Logger.LogInformation(
                "{Entity} successfully updated with Date={Date} BarberShopId={BarberShopId}",
                Entity, date, barberShopId);
            
        internal void DeletingStart(DateOnly date, int barberShopId) =>
            Logger.LogInformation
            ("Received request to delete {Entity} Date={Date} BarberShopId={BarberShopId}",
                Entity, date, barberShopId);

        internal void Deleted(DateOnly date, int barberShopId) =>
            Logger.LogInformation(
                "{Entity} successfully deleted with Date={Date} BarberShopId={BarberShopId}",
                Entity, date, barberShopId);
    }
    
    public static async Task<Results<Created<SpecialScheduleDtoResponse>, BadRequest<Error>>> CreateSpecialScheduleAsync(
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        SpecialScheduleService service,
        SpecialScheduleErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var schedule = await service.CreateAsync(dto, cancellationToken);

        if (schedule is null)
            return errors.Create();

        logger.Created(schedule.Date, schedule.BarberShopId);
        return TypedResults.Created(GetBaseEndpoint(schedule), schedule);
    }

    public static async Task<Results<Ok<SpecialScheduleDtoResponse>, NotFound<Error>>> GetSpecialScheduleAsync(
        int id,
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service,
        SpecialScheduleErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(date, barberShopId);

        var schedule = await service.GetByIdAsync(id, cancellationToken);

        if (schedule is null)
            return errors.NotFound();

        if (schedule.Date != date || schedule.BarberShopId != barberShopId)
            return errors.NotFound();
            
        return TypedResults.Ok(schedule);
    }

    public static async Task<Ok<PaginationResponse<SpecialScheduleDtoResponse>>> GetAllSpecialSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        SpecialScheduleService service,
        SpecialScheduleErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var schedules = await service.GetAllAsync(page, pageSize, barberShopId, cancellationToken);
        return TypedResults.Ok(schedules);
    }
    
    public static async Task<Results<Ok<SpecialScheduleDtoResponse>, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> UpdateSpecialScheduleAsync(
        int id,
        DateOnly date,
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        SpecialScheduleService service,
        SpecialScheduleErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.UpdatingStart(date, barberShopId, dto);
        
        var infos = await service.GetEntityInfosAsync(id, date, barberShopId, cancellationToken);
        
        if (!infos.Exists)
            return errors.NotFound();

        if (!infos.BelongsToCurrentUser)
            return errors.SpecialScheduleBelongsToAnotherBarberShop();

        if (!infos.BelongsToBarberShop)
            return errors.SpecialScheduleBelongsToAnotherBarberShop();

        if (!infos.BelongsToDay)
            return errors.SpecialScheduleBelongsToAnotherDay();

        var schedule = await service.UpdateAsync(dto, id, cancellationToken);

        if (schedule is null)
            return errors.Update();
            
        logger.Updated(date, barberShopId);
        return TypedResults.Ok(schedule);
    }

    public static async Task<Results<NoContent, NotFound<Error>, Conflict<Error>, BadRequest<Error>>> DeleteSpecialScheduleAsync(
        int id,
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service,
        SpecialScheduleErrors errors,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var logger = LoggerActions.FactoryCreate(loggerFactory);
        
        logger.DeletingStart(date, barberShopId);
        
        var infos = await service.GetEntityInfosAsync(id, date, barberShopId, cancellationToken);
        
        if (!infos.Exists)
            return errors.NotFound();
            
        if (!infos.BelongsToCurrentUser)
            return errors.SpecialScheduleBelongsToAnotherBarberShop();

        if (!infos.BelongsToBarberShop)
            return errors.SpecialScheduleBelongsToAnotherBarberShop();

        if (!infos.BelongsToDay)
            return errors.SpecialScheduleBelongsToAnotherDay();
            
        if (!await service.DeleteAsync(id, cancellationToken))
            return errors.Delete();
            
        logger.Deleted(date, barberShopId);
        return TypedResults.NoContent();
    }
}
