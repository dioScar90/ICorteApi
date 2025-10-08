using ICorteApi.Application.Services;
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

        group.MapGet("{date}", GetSpecialScheduleAsync)
            .WithSummary("Get Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllSpecialSchedulesAsync)
            .WithSummary("Get All Special Schedules")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPut("{date}", UpdateSpecialScheduleAsync)
            .WithSummary("Update Special Schedule")
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));

        group.MapDelete("{date}", DeleteSpecialScheduleAsync)
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
    
    public static async Task<Created<SpecialScheduleDtoResponse>> CreateSpecialScheduleAsync(
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var schedule = await service.CreateAsync(dto);

        logger.Created(schedule.Date, schedule.BarberShopId);
        return TypedResults.Created(GetBaseEndpoint(schedule), schedule);
    }

    public static async Task<Ok<SpecialScheduleDtoResponse>> GetSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(date, barberShopId);

        var schedule = await service.GetByIdAsync(date, barberShopId);
        return TypedResults.Ok(schedule);
    }

    public static async Task<Ok<PaginationResponse<SpecialScheduleDtoResponse>>> GetAllSpecialSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return TypedResults.Ok(schedules);
    }

    public static async Task<NoContent> UpdateSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleDtoRequest dto,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.UpdatingStart(date, barberShopId, dto);

        await service.UpdateAsync(dto, date, barberShopId);

        logger.Updated(date, barberShopId);
        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(date, barberShopId);

        await service.DeleteAsync(date, barberShopId);

        logger.Deleted(date, barberShopId);
        return TypedResults.NoContent();
    }
}
