namespace BarberAppApi.Dtos;

public record MessageDto(
    string Content,
    DateTime Timestamp,
    bool IsReal
);
