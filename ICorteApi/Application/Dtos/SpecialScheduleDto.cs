using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoCreate(
    DateOnly Date,
    string? Notes = null,
    TimeOnly? OpenTime = null,
    TimeOnly? CloseTime = null,
    bool? IsClosed = null
) : IDtoRequest<SpecialSchedule>;

public record SpecialScheduleDtoUpdate(
    DateOnly Date,
    string? Notes = null,
    TimeOnly? OpenTime = null,
    TimeOnly? CloseTime = null,
    bool? IsClosed = null
) : IDtoRequest<SpecialSchedule>;

public record SpecialScheduleDtoResponse(
    DateOnly Date,
    int BarberShopId,
    string? Notes,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoResponse<SpecialSchedule>;
