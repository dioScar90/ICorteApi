using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Enums;
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

        group.MapPost(INDEX, CreatePayment);
        group.MapGet("{id}", GetPayment);
        group.MapGet(INDEX, GetAllPayments);
        group.MapDelete("{id}", DeletePayment);

        return app;
    }

    public static IResult GetCreatedResult(int newId, int appointmentId)
    {
        string uri = EndpointPrefixes.Appointment + "/" + appointmentId + "/" + EndpointPrefixes.Payment + "/" + newId;
        object value = new { Message = "Pagamento criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> CreatePayment(
        int appointmentId,
        PaymentDtoCreate dto,
        IPaymentService service)
    {
        var payment = await service.CreateAsync(dto, appointmentId);
        return GetCreatedResult(payment.Id, payment.AppointmentId);
    }

    public static async Task<IResult> GetPayment(
        int id,
        int appointmentId,
        IPaymentService service)
    {
        var payment = await service.GetByIdAsync(id, appointmentId);
        return Results.Ok(payment);
    }

    public static async Task<IResult> GetAllPayments(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int appointmentId,
        IPaymentService service)
    {
        var payments = await service.GetAllAsync(page, pageSize, appointmentId);
        return Results.Ok(payments);
    }

    public static async Task<IResult> DeletePayment(
        int id,
        int appointmentId,
        IPaymentService service)
    {
        await service.DeleteAsync(id, appointmentId);
        return Results.NoContent();
    }
}
