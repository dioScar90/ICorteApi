namespace ICorteApi.Dtos;

public record ServiceDto(
    string ServiceName,
    string Description,
    decimal Price,
    TimeSpan Duration
);
