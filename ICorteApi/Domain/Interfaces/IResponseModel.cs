namespace ICorteApi.Domain.Interfaces;

public interface IResponseModel
{
    bool Success { get; }
    string? Message { get; }
    int? StatusCode { get; }

}
