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

    public static IResult GetCreatedResult(int newId, int barberShopId) =>
        Results.Created($"barber-shop/{barberShopId}/report/{newId}", new { Message = "Pagamento criado com sucesso" });

    public static async Task<IResult> CreateReportAsync(
        int barberShopId,
        ReportDtoCreate dto,
        IReportService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        var report = await service.CreateAsync(dto, userId, barberShopId);
        return GetCreatedResult(report.Id, report.BarberShopId);
    }

    public static async Task<IResult> GetReportAsync(
        int id,
        int barberShopId,
        IReportService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var report = await service.GetByIdAsync(id, clientId, barberShopId);
        return Results.Ok(report);
    }

    public static async Task<IResult> GetAllReportsAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IReportService service)
    {
        var reports = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(reports);
    }

    public static async Task<IResult> UpdateReportAsync(
        int id,
        int barberShopId,
        ReportDtoUpdate dto,
        IReportService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.UpdateAsync(dto, id, clientId, barberShopId);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteReportAsync(
        int id,
        int barberShopId,
        IReportService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, clientId, barberShopId);
        return Results.NoContent();
    }
}
