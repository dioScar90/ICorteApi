using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoRequest(
    DateOnly Date,
    TimeSpan? OpenTime,
    TimeSpan? CloseTime,
    bool IsClosed,
    string? Notes
) : IDtoRequest;

public record SpecialScheduleDtoResponse(
    int Id,
    DateOnly Date,
    TimeSpan? OpenTime,
    TimeSpan? CloseTime,
    bool IsClosed,
    string? Notes
) : IDtoResponse;
