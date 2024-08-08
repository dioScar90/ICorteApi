using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Dtos;

public record PaymentDtoRequest(
    PaymentType PaymentType,
    decimal Amount
) : IDtoRequest<Payment>;

public record PaymentDtoResponse(
    int Id,
    PaymentType PaymentType,
    decimal Amount
) : IDtoResponse<Payment>;
