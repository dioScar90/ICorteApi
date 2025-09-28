using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoResponse(
    DateOnly Date,
    int BarberShopId,
    DayOfWeek DayOfWeek,
    string? Notes = null,
    TimeOnly? OpenTime = null,
    TimeOnly? CloseTime = null,
    bool IsClosed = false
) : IDtoResponse<SpecialSchedule>;

public record SpecialScheduleDtoRequest(
    DateOnly Date,
    int BarberShopId,
    DayOfWeek DayOfWeek,
    string? Notes = null,
    TimeOnly? OpenTime = null,
    TimeOnly? CloseTime = null,
    bool IsClosed = false
) : IDtoRequest<SpecialSchedule>;
