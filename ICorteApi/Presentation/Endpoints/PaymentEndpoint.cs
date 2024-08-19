using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class PaymentEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Appointment + "{appointmentId}" + EndpointPrefixes.Payment;
    private static readonly string ENDPOINT_NAME = EndpointNames.Payment;

    public static IEndpointRouteBuilder MapPaymentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllPayments);
        group.MapGet("{id}", GetPayment);
        group.MapPost(INDEX, CreatePayment);
        group.MapPut("{id}", UpdatePayment);
        group.MapDelete("{id}", DeletePayment);

        return app;
    }
    
    public static IResult GetCreatedResult(int newId, int appointmentId)
    {
        string uri = EndpointPrefixes.Appointment + "/" + appointmentId + "/" + EndpointPrefixes.Payment + "/" + newId;
        object value = new { Message = "Pagamento criado com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> GetPayment(
        int id,
        IPaymentService service,
        IPaymentErrors errors)
    {
        var res = await service.GetByIdAsync(id);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        return Results.Ok(res.Value!.CreateDto());
    }

    public static async Task<IResult> GetAllPayments(
        int? page,
        int? pageSize,
        IPaymentService service,
        IPaymentErrors errors)
    {
        var res = await service.GetAllAsync(page, pageSize);

        if (!res.IsSuccess)
            errors.ThrowBadRequestException(res.Error);
            
        var dtos = res.Values!
            .Select(c => c.CreateDto())
            .ToList();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreatePayment(
        int appointmentId,
        PaymentDtoRequest dto,
        IValidator<PaymentDtoRequest> validator,
        IPaymentService service,
        IPaymentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var res = await service.CreateAsync(dto, appointmentId);

        if (!res.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(res.Value!.Id, appointmentId);
    }

    public static async Task<IResult> UpdatePayment(
        int id,
        PaymentDtoRequest dto,
        IValidator<PaymentDtoRequest> validator,
        IPaymentService service,
        IPaymentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        
        var res = await service.UpdateAsync(dto, id);

        if (!res.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeletePayment(
        int id,
        IPaymentService service,
        IPaymentErrors errors)
    {
        var res = await service.DeleteAsync(id);

        if (!res.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
