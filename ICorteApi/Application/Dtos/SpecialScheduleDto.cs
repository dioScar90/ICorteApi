using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoRequest(
    DateOnly Date,
    string? Notes,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoRequest<SpecialSchedule>;

public record SpecialScheduleDtoResponse(
    int Id,
    string? Notes,
    DateOnly Date,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoResponse<SpecialSchedule>;
