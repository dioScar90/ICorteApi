using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record ReportDtoResponse(
    int Id,
    int BarberShopId,
    string? Title,
    string? Content,
    int Rating
) : IDtoResponse<Report>;

public record ReportDtoRequest(
    int Id,
    int BarberShopId,
    string? Title,
    string? Content,
    int Rating
) : IDtoRequest<Report>;
