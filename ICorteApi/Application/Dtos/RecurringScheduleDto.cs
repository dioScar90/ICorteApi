namespace ICorteApi.Application.Dtos;

public record RecurringScheduleDtoCreate(
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime
) : IDtoRequest<RecurringSchedule>;

public record RecurringScheduleDtoUpdate(
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoRequest<RecurringSchedule>;

public record RecurringScheduleDtoResponse(
    DayOfWeek DayOfWeek,
    int BarberShopId,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoResponse<RecurringSchedule>;
