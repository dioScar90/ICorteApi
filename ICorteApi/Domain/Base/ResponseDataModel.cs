using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public record ResponseDataModel<T>(
    bool Success,
    T? Data = default,
    string? Message = default,
    int? StatusCode = default
) : ResponseModel(
    Success, Message, StatusCode
), IResponseDataModel<T> where T : class;
