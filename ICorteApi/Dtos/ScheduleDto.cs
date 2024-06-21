namespace BarberAppApi.Dtos;

public record ScheduleDto(
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime
);
