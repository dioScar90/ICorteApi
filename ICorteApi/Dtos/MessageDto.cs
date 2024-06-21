namespace ICorteApi.Dtos;

public record MessageDto(
    string Content,
    DateTime Timestamp,
    bool IsReal
);
