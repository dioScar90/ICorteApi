using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Interfaces;

public interface ICollectionResponse<T> : IResponse where T : IBaseTableEntity
{
    ICollection<T>? Values { get; }
    // ICollectionResponse<TValue> Success<TValue>(ICollection<TValue> values) where TValue : IBaseTableEntity;
    // ICollectionResponse<TValue> FailureCollection<TValue>(Error error) where TValue : IBaseTableEntity;
}
