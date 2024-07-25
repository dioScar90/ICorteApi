using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record ServiceDtoRequest(
    string Name,
    string Description,
    decimal Price,
    TimeSpan Duration
) : IDtoRequest<Service>;

public record ServiceDtoResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse<Service>;
