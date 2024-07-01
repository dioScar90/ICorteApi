namespace ICorteApi.Application.Dtos;

public record TimeSlotDtoResponse(
    TimeSpan StartTime,
    TimeSpan EndTime
);
