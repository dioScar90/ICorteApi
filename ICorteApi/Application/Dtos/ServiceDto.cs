using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record ServiceDtoRequest(
    string Name,
    string Description,
    decimal Price,
    TimeSpan Duration
) : IDtoRequest;

public record ServiceDtoResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse;
