namespace ICorteApi.Dtos;

public record ScheduleDtoRequest(
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsAvailable
) : IDtoRequest;

public record ScheduleDtoResponse(
    int Id,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsAvailable
) : IDtoResponse;
