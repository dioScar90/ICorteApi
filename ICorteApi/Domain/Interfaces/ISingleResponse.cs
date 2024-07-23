using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface ISingleResponse<T> : IResponse where T : class, IBaseTableEntity
{
    T? Value { get; }
}
