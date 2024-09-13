using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ReportEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "{barberShopId}" + EndpointPrefixes.Report;
    private static readonly string ENDPOINT_NAME = EndpointNames.Report;

    public static IEndpointRouteBuilder MapReportEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost(INDEX, CreateReport)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapGet("{id}", GetReport)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapGet(INDEX, GetAllReports)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapPut("{id}", UpdateReport)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapDelete("{id}", DeleteReport)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        return app;
    }

    public static IResult GetCreatedResult(int newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.Report + "/" + newId;
        object value = new { Message = "Pagamento criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreateReport(
        int barberShopId,
        ReportDtoCreate dto,
        IReportService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        var report = await service.CreateAsync(dto, userId, barberShopId);
        return GetCreatedResult(report.Id, report.BarberShopId);
    }

    public static async Task<IResult> GetReport(
        int id,
        int barberShopId,
        IReportService service,
        IUserService userService)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var report = await service.GetByIdAsync(id, clientId, barberShopId);
        return Results.Ok(report);
    }

    public static async Task<IResult> GetAllReports(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IReportService service)
    {
        var reports = await service.GetAllAsync(page, pageSize, barberShopId);
        return Results.Ok(reports);
    }

    public static async Task<IResult> UpdateReport(
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

    public static async Task<IResult> DeleteReport(
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
