using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class SpecialScheduleEndpoint
{
    public static IEndpointRouteBuilder MapSpecialScheduleEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/special-schedule").WithTags("Special Schedule");

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
    
    private static IResult GetCreatedResult(SpecialScheduleDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/special-schedule/{dto.Date}", new { Message = "Horário especial criado com sucesso", Item = dto });

    public static async Task<IResult> CreateSpecialScheduleAsync(
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
        return GetCreatedResult(schedule);
    }

    public static async Task<IResult> GetSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(date, barberShopId);

        var schedule = await service.GetByIdAsync(date, barberShopId);
        return Results.Ok(schedule);
    }

    public static async Task<IResult> GetAllSpecialSchedulesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var schedules = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(schedules);
    }

    public static async Task<IResult> UpdateSpecialScheduleAsync(
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
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSpecialScheduleAsync(
        DateOnly date,
        int barberShopId,
        SpecialScheduleService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(date, barberShopId);

        await service.DeleteAsync(date, barberShopId);

        logger.Deleted(date, barberShopId);
        return Results.NoContent();
    }
}
