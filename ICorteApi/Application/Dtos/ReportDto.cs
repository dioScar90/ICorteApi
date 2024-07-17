using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Dtos;

public record ReportDtoRequest(
    string? Title,
    string? Content,
    Rating Rating
) : IDtoRequest;

public record ReportDtoResponse(
    int Id,
    string? Title,
    string? Content,
    Rating Rating
) : IDtoResponse;
