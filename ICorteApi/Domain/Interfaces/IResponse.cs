using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IResponse
{
    bool IsSuccess { get; }
    Error Error { get; }
}
