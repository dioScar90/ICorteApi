using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record BarberShopDtoCreate(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoCreate? Address = null,
    RecurringScheduleDtoRequest[]? RecurringSchedules = null,
    SpecialScheduleDtoRequest[]? SpecialSchedules = null,
    ServiceDtoRequest[]? Services = null,
    ReportDtoRequest[]? Reports = null
) : IDtoRequest<BarberShop>;

public record BarberShopDtoUpdate(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoCreate? Address = null
) : IDtoRequest<BarberShop>;

public record BarberShopDtoResponse(
    int Id,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoResponse? Address,
    RecurringScheduleDtoResponse[]? RecurringSchedules,
    SpecialScheduleDtoResponse[]? SpecialSchedules,
    ServiceDtoResponse[]? Services,
    ReportDtoResponse[]? Reports
) : IDtoResponse<BarberShop>;
