using System.Diagnostics.CodeAnalysis;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

// This was made using record (I do prefer).
// If want to use it by classical classes, uncomment the codes bellow.
public abstract record Response : IResponse
{
    protected Response(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public Error Error { get; }

    public static Response Success() => new SuccessResponse();

    public static SingleResponse<TValue> Success<TValue>(TValue value)
        where TValue : IBaseTableEntity => new(value, true, Error.None);

    public static CollectionResponse<TValue> Success<TValue>(ICollection<TValue> values)
        where TValue : IBaseTableEntity => new(values, true, Error.None);
    
    public static CollectionResponseWithPagination<TValue> Success<TValue>(
        ICollection<TValue> values, int totalItems, int totalPages, int currentPage, int pageSize)
        where TValue : IBaseTableEntity => new(values, true, Error.None, int totalItems, int totalPages, int currentPage, int pageSize);

    public static Response Failure(Error error) => new FailureResponse(error);

    public static SingleResponse<TValue> Failure<TValue>(Error error)
        where TValue : IBaseTableEntity => new(default, false, error);

    public static CollectionResponse<TValue> FailureCollection<TValue>(Error error)
        where TValue : IBaseTableEntity => new(default, false, error);
}

public record SingleResponse<TValue>(TValue? Value, bool IsSuccess, Error Error)
    : Response(IsSuccess, Error), ISingleResponse<TValue> where TValue : IBaseTableEntity
{
    [NotNull]
    public TValue? Value { get; } = Value;
}

public record CollectionResponseWithPagination<TValue>(
    ICollection<TValue> Values, bool IsSuccess, Error Error,
    int TotalItems, int TotalPages, int CurrentPage, int PageSize)
    : Response(IsSuccess, Error), ICollectionResponseWithPagination<TValue> where TValue : IBaseTableEntity
{
    [NotNull]
    public ICollection<TValue> Values { get; } = Values;
    public int TotalItems { get; } = TotalItems;
    public int TotalPages { get; } = TotalPages;
    public int CurrentPage { get; } = CurrentPage;
    public int PageSize { get; } = PageSize;
}

public record CollectionResponse<TValue>(
    ICollection<TValue> Values, bool IsSuccess, Error Error)
    : Response(IsSuccess, Error), ICollectionResponse<TValue> where TValue : IBaseTableEntity
{
    [NotNull]
    public ICollection<TValue> Values { get; } = Values;
}

public record SuccessResponse()
    : Response(true, Error.None)
{
}

public record FailureResponse(Error Error)
    : Response(false, Error)
{
}


// public abstract class Response : IResponse
// {
//     protected Response(bool isSuccess, Error error)
//     {
//         if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
//             throw new ArgumentException("Invalid error", nameof(error));

//         IsSuccess = isSuccess;
//         Error = error;
//     }

//     public bool IsSuccess { get; }

//     public Error Error { get; }

//     public static Response Success() => new SuccessResponse();

//     public static SingleResponse<TValue> Success<TValue>(TValue value)
//         where TValue : IBaseTableEntity => new(value, true, Error.None);

//     public static CollectionResponse<TValue> Success<TValue>(ICollection<TValue> values)
//         where TValue : IBaseTableEntity => new(values, true, Error.None);

//     public static Response Failure(Error error) => new FailureResponse(error);

//     public static SingleResponse<TValue> Failure<TValue>(Error error)
//         where TValue : IBaseTableEntity => new(default, false, error);

//     public static CollectionResponse<TValue> FailureCollection<TValue>(Error error)
//         where TValue : IBaseTableEntity => new(default, false, error);
// }

// public class SingleResponse<TValue>(TValue? value, bool isSuccess, Error error)
//     : Response(isSuccess, error), ISingleResponse<TValue> where TValue : IBaseTableEntity
// {
//     [NotNull]
//     public TValue? Value { get; } = value;

//     public static implicit operator SingleResponse<TValue>(TValue? value) =>
//         value is not null
//             ? new SingleResponse<TValue>(value, true, Error.None)
//             : new SingleResponse<TValue>(default, false, Error.NullValue);
// }

// public class CollectionResponse<TValue>(ICollection<TValue>? values, bool isSuccess, Error error)
//     : Response(isSuccess, error), ICollectionResponse<TValue> where TValue : IBaseTableEntity
// {
//     [NotNull]
//     public ICollection<TValue>? Values { get; } = values;

//     public static CollectionResponse<TValue> FromValues(ICollection<TValue>? values) =>
//         values is not null
//             ? new CollectionResponse<TValue>(values, true, Error.None)
//             : new CollectionResponse<TValue>(default, false, Error.NullValue);
// }

// public class SuccessResponse()
//     : Response(true, Error.None)
// {
// }

// public class FailureResponse(Error error)
//     : Response(false, error)
// {
// }
