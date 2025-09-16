namespace ICorteApi.Application.Dtos;

public record ServiceDto(
    int Id,
    int BarberShopId,
    string? BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDto<Service>;
