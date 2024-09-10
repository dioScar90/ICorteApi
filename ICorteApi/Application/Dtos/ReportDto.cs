using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record ReportDtoCreate(
    string? Title,
    string? Content,
    int Rating
) : IDtoRequest<Report>;

public record ReportDtoUpdate(
    string? Title,
    string? Content,
    int Rating
) : IDtoRequest<Report>;

public record ReportDtoResponse(
    int Id,
    int BarberShopId,
    string? Title,
    string? Content,
    int Rating
) : IDtoResponse<Report>;
