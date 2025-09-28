using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ReportEndpoint
{
    public static IEndpointRouteBuilder MapReportEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("barber-shop/{barberShopId}/report").WithTags("Report");

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
    
    private static IResult GetCreatedResult(ReportDtoResponse dto) =>
        Results.Created($"barber-shop/{dto.BarberShopId}/report/{dto.Id}", new { Message = "Pagamento criado com sucesso", Item = dto });

    public static async Task<IResult> CreateReportAsync(
        int barberShopId,
        ReportDtoRequest dto,
        ReportService service)
    {
        dto = dto with { BarberShopId = barberShopId };
        var report = await service.CreateAsync(dto);
        return GetCreatedResult(report);
    }

    public static async Task<IResult> GetReportAsync(
        int id,
        int barberShopId,
        ReportService service)
    {
        var report = await service.GetByIdAsync(id, barberShopId);
        return Results.Ok(report);
    }

    public static async Task<IResult> GetAllReportsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        ReportService service)
    {
        var reports = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(reports);
    }

    public static async Task<IResult> UpdateReportAsync(
        int id,
        int barberShopId,
        ReportDtoRequest dto,
        ReportService service)
    {
        await service.UpdateAsync(dto, id, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteReportAsync(
        int id,
        int barberShopId,
        ReportService service)
    {
        await service.DeleteAsync(id, barberShopId);
        return Results.NoContent();
    }
}
