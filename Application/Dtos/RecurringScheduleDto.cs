using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record RecurringScheduleDtoResponse(
    DayOfWeek DayOfWeek,
    int BarberShopId,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoResponse<RecurringSchedule>;

public record RecurringScheduleDtoRequest(
    DayOfWeek DayOfWeek,
    int BarberShopId,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoRequest<RecurringSchedule>;
