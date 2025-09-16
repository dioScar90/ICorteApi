namespace ICorteApi.Application.Dtos;

public record ReportDto(
    int Id,
    int BarberShopId,
    string? Title,
    string? Content,
    int Rating
) : IDto<Report>;
