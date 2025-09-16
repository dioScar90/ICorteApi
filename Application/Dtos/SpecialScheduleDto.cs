namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDto(
    DateOnly Date,
    int BarberShopId,
    DayOfWeek DayOfWeek,
    string? Notes = null,
    TimeOnly? OpenTime = null,
    TimeOnly? CloseTime = null,
    bool IsClosed = false
) : IDto<SpecialSchedule>;
