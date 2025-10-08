using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ReportEndpoint
{
    private static string GetBaseEndpoint(ReportDtoResponse? report = null) => report is null
        ? "barber-shop/{barberShopId}/report"
        : $"barber-shop/{report.BarberShopId}/report/{report.Id}";

    public static IEndpointRouteBuilder MapReportEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Report");

        group.MapPost("", CreateReportAsync)
            .WithSummary("Create Report")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapGet("{id}", GetReportAsync)
            .WithSummary("Get Report")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapGet("", GetAllReportsAsync)
            .WithSummary("Get All Reports")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapPut("{id}", UpdateReportAsync)
            .WithSummary("Update Report")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapDelete("{id}", DeleteReportAsync)
            .WithSummary("Delete Report")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

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
                Entity = nameof(Report),
                Logger = loggerFactory.CreateLogger(nameof(ReportEndpoint))
            };
        }

        internal void CreatingStart(ReportDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);

        internal void GettingAllStart(int barberShopId, int? page, int? pageSize) =>
            Logger.LogInformation(
                "Received request to get all {Entity} with BarberShopId={BarberShopId} Page={Page} PageSize={PageSize}",
                Entity, barberShopId, page, pageSize);

        internal void UpdatingStart(int id, ReportDtoRequest dto) =>
            Logger.LogInformation("Received request to update {Entity} with Id={Id} {@Entity}", Entity, id, dto);

        internal void Updated(int id) =>
            Logger.LogInformation("{Entity} successfully updated with Id={Id}", Entity, id);
            
        internal void DeletingStart(int id) =>
            Logger.LogInformation("Received request to delete {Entity} Id={Id}", Entity, id);

        internal void Deleted(int id) =>
            Logger.LogInformation("{Entity} successfully deleted with Id={Id}", Entity, id);
    }
    
    public static async Task<Created<ReportDtoResponse>> CreateReportAsync(
        int barberShopId,
        ReportDtoRequest dto,
        ReportService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { BarberShopId = barberShopId };
        logger.CreatingStart(dto);

        var report = await service.CreateAsync(dto);

        logger.Created(report.Id);
        return TypedResults.Created(GetBaseEndpoint(report), report);
    }

    public static async Task<Ok<ReportDtoResponse>> GetReportAsync(
        int id,
        int barberShopId,
        ReportService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var report = await service.GetByIdAsync(id, barberShopId);
        return TypedResults.Ok(report);
    }

    public static async Task<Ok<PaginationResponse<ReportDtoResponse>>> GetAllReportsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ReportService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(barberShopId, page, pageSize);

        var reports = await service.GetAllAsync(page, pageSize, barberShopId);
        return TypedResults.Ok(reports);
    }

    public static async Task<NoContent> UpdateReportAsync(
        int id,
        int barberShopId,
        ReportDtoRequest dto,
        ReportService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.UpdatingStart(id, dto);

        await service.UpdateAsync(dto, id, barberShopId);

        logger.Updated(id);
        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteReportAsync(
        int id,
        int barberShopId,
        ReportService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(id);

        await service.DeleteAsync(id, barberShopId);

        logger.Deleted(id);
        return TypedResults.NoContent();
    }
}
