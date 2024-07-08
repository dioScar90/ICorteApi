using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public record ResponseModel(
    bool Success,
    string? Message = default,
    int? StatusCode = default
) : IResponseModel;
