using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record ReportDtoRequest(
    string? Title,
    string? Content,
    int Rating
) : IDtoRequest;

public record ReportDtoResponse(
    int Id,
    string? Title,
    string? Content,
    int Rating
) : IDtoResponse;
