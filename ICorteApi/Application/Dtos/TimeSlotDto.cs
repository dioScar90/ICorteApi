namespace ICorteApi.Dtos;

public record TimeSlotDtoResponse(
    TimeSpan StartTime,
    TimeSpan EndTime
);
