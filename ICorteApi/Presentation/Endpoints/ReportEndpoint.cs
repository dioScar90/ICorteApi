using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Domain.Enums;
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

        group.MapGet(INDEX, GetAllReports)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOnly));

        group.MapGet("{id}", GetReport)
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

    public static async Task<IResult> GetReport(
        int barberShopId,
        int id,
        IReportService service,
        IUserService userService,
        IReportErrors errors)
    {
        int clientId = userService.GetMyUserId();
        var res = await service.GetByIdAsync(id, clientId, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        return Results.Ok(res.Value!.CreateDto());
    }

    public static async Task<IResult> GetAllReports(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int barberShopId,
        IReportService service,
        IReportErrors errors)
    {
        var res = await service.GetAllAsync(page, pageSize, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowBadRequestException(res.Error);

        var dtos = res.Values!
            .Select(c => c.CreateDto())
            .ToList();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> UpdateReport(
        int barberShopId,
        int id,
        ReportDtoRequest dto,
        IValidator<ReportDtoRequest> validator,
        IReportService service,
        IUserService userService,
        IReportErrors errors)
    {
        int clientId = userService.GetMyUserId();
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var res = await service.UpdateAsync(dto, id, clientId, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteReport(
        int barberShopId,
        int id,
        IReportService service,
        IUserService userService,
        IReportErrors errors)
    {
        int clientId = userService.GetMyUserId();
        var res = await service.DeleteAsync(id, clientId, barberShopId);

        if (!res.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
