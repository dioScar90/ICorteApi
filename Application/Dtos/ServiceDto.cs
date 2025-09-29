using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record ServiceDtoResponse(
    int Id,
    int BarberShopId,
    string? BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse<Service>;

public record ServiceDtoRequest(
    int BarberShopId,
    string? BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDtoRequest<Service>;

public record ServiceByNameDtoResponse(
    int Id,
    int BarberShopId,
    string BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse<Service>;
