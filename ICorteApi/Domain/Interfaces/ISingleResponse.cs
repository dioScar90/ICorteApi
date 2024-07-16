using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface ISingleResponse<T> : IResponse where T : IBaseTableEntity
{
    T? Value { get; }
    // ISingleResponse<TValue> Success<TValue>(TValue value) where TValue : IBaseTableEntity;
    // ISingleResponse<TValue> Failure<TValue>(Error error) where TValue : IBaseTableEntity;
}
