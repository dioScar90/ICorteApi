using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class ReportEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "{barberShopId}" + EndpointPrefixes.Report;
    private static readonly string ENDPOINT_NAME = EndpointNames.Report;

    public static IEndpointRouteBuilder MapReportEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapGet(INDEX, GetAllReports)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetReport)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPost(INDEX, CreateReport);
        group.MapPut("{id}", UpdateReport);
        group.MapDelete("{id}", DeleteReport);

        return app;
    }

    private static int GetMyUserId(this IUserService service) => (int)service.GetMyUserIdAsync()!;
    
    public static IResult GetCreatedResult(int newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.Report + "/" + newId;
        object value = new { Message = "Pagamento criado com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> GetReport(
        int id,
        IReportService service,
        IReportErrors errors)
    {
        var res = await service.GetByIdAsync(id);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        return Results.Ok(res.Value!.CreateDto());
    }

    public static async Task<IResult> GetAllReports(
        int? page,
        int? pageSize,
        IReportService service,
        IReportErrors errors)
    {
        var res = await service.GetAllAsync(page, pageSize);

        if (!res.IsSuccess)
            errors.ThrowBadRequestException(res.Error.Description);
            
        var dtos = res.Values!
            .Select(c => c.CreateDto())
            .ToList();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateReport(
        int barberShopId,
        ReportDtoRequest dto,
        IValidator<ReportDtoRequest> validator,
        IReportService service,
        IUserService userService,
        IReportErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int userId = userService.GetMyUserId();
        var res = await service.CreateAsync(dto, userId, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(res.Value!.Id, barberShopId);
    }

    public static async Task<IResult> UpdateReport(
        int id,
        ReportDtoRequest dto,
        IValidator<ReportDtoRequest> validator,
        IReportService service,
        IReportErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        
        var res = await service.UpdateAsync(dto, id);

        if (!res.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteReport(
        int id,
        IReportService service,
        IReportErrors errors)
    {
        var res = await service.DeleteAsync(id);

        if (!res.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
