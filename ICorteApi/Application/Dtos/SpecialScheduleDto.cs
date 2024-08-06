using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoRequest(
    DateOnly Date,
    int? BarberShopId,
    string? Notes,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoRequest<SpecialSchedule>;

public record SpecialScheduleDtoResponse(
    DateOnly Date,
    int BarberShopId,
    string? Notes,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoResponse<SpecialSchedule>;
