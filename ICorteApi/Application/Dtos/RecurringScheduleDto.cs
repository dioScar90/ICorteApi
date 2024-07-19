using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record RecurringScheduleDtoRequest(
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoRequest;

public record RecurringScheduleDtoResponse(
    DayOfWeek DayOfWeek,
    int BarberShopId,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoResponse;
