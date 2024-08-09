using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record BarberShopDtoRequest(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoRequest? Address,
    RecurringScheduleDtoRequest[]? RecurringSchedules,
    UserDtoRequest[]? Barbers
) : IDtoRequest<BarberShop>;

public record BarberShopDtoResponse(
    int Id,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoResponse? Address,
    RecurringScheduleDtoResponse[]? RecurringSchedules,
    UserDtoResponse[]? Barbers
) : IDtoResponse<BarberShop>;
