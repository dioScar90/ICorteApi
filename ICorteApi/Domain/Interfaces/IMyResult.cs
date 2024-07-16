using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface IResponse
{
    bool IsSuccess { get; }
    Error Error { get; }
    // ISingleResponse<TValue> Success<TValue>(TValue value) where TValue : IBaseTableEntity;
    // ICollectionResponse<TValue> Success<TValue>(ICollection<TValue> values) where TValue : IBaseTableEntity;
    // ISingleResponse<TValue> Failure<TValue>(Error error) where TValue : IBaseTableEntity;
    // ICollectionResponse<TValue> FailureCollection<TValue>(Error error) where TValue : IBaseTableEntity;
}
