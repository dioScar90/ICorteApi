namespace ICorteApi.Application.Dtos;

public record ServiceDtoCreate(
    string Name,
    string Description,
    decimal Price,
    TimeSpan Duration
) : IDtoRequest<Service>;

public record ServiceDtoUpdate(
    string Name,
    string Description,
    decimal Price,
    TimeSpan Duration
) : IDtoRequest<Service>;

public record ServiceDtoResponse(
    int Id,
    int BarberShopId,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse<Service>;

public record ServiceByNameDtoResponse(
    int Id,
    int BarberShopId,
    string BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse<Service>;
