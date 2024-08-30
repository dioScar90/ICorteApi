using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

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
        int appointmentId,
        int id,
        IPaymentService service,
        IPaymentErrors errors)
    {
        var payment = await service.GetByIdAsync(id, appointmentId);

        if (payment is null)
            errors.ThrowNotFoundException();

        return Results.Ok(payment!.CreateDto());
    }

    public static async Task<IResult> GetAllPayments(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int appointmentId,
        IPaymentService service,
        IPaymentErrors errors)
    {
        var payments = await service.GetAllAsync(page, pageSize, appointmentId);
        
        var dtos = payments?.Select(p => p.CreateDto()).ToArray() ?? [];
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
        var payment = await service.CreateAsync(dto, appointmentId);

        if (payment is null)
            errors.ThrowCreateException();

        return GetCreatedResult(payment!.Id, payment.AppointmentId);
    }
    
    public static async Task<IResult> DeletePayment(
        int appointmentId,
        int id,
        IPaymentService service,
        IPaymentErrors errors)
    {
        var result = await service.DeleteAsync(id, appointmentId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
