namespace ICorteApi.Application.Dtos;

public record RecurringScheduleDto(
    DayOfWeek DayOfWeek,
    int BarberShopId,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDto<RecurringSchedule>;
