using System.Collections.Frozen;
using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record BarberShopDtoRequest(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoRequest? Address,
    RecurringScheduleDtoRequest[]? RecurringSchedules,
    PersonDtoRequest[]? Barbers
) : IDtoRequest;

public record BarberShopDtoResponse(
    int Id,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoResponse? Address,
    RecurringScheduleDtoResponse[]? RecurringSchedules,
    PersonDtoResponse[]? Barbers
) : IDtoResponse;
