using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoRequest(
    DateOnly Date,
    string? Notes,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoRequest;

public record SpecialScheduleDtoResponse(
    int Id,
    string? Notes,
    DateOnly Date,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoResponse;
