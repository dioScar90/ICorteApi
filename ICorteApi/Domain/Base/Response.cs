using System.Diagnostics.CodeAnalysis;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public abstract record Response : IResponse
{
    protected Response(bool isSuccess, params Error[]? error)
    {
        // if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
        //     throw new ArgumentException("Invalid error", nameof(error));
        
        if (!IsValidResponseConstruction(isSuccess, error))
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    private static bool IsValidResponseConstruction(bool isSuccess, Error[]? error)
    {
        if (isSuccess && error is { Length: > 0 })
            return false;

        if (!isSuccess && error is not { Length: > 0 })
            return false;

        return true;
    }

    public bool IsSuccess { get; }

    public Error[]? Error { get; }

    public static Response Success() => new SuccessResponse();

    public static SingleResponse<TValue> Success<TValue>(TValue value)
        where TValue : class, IBaseTableEntity => new(value, true);

    public static CollectionResponse<TValue> Success<TValue>(ICollection<TValue> values)
        where TValue : class, IBaseTableEntity => new(values, true);
    
    public static CollectionResponseWithPagination<TValue> Success<TValue>(
        ICollection<TValue> values, int totalItems, int totalPages, int currentPage, int pageSize)
        where TValue : class, IBaseTableEntity => new(values, true, totalItems, totalPages, currentPage, pageSize);

    public static Response Failure(params Error[]? error) => new FailureResponse(error);

    public static SingleResponse<TValue> Failure<TValue>(params Error[]? error)
        where TValue : class, IBaseTableEntity => new(default, false, error);

    public static CollectionResponse<TValue> FailureCollection<TValue>(params Error[]? error)
        where TValue : class, IBaseTableEntity => new(default, false, error);
}

public record SingleResponse<TValue>(TValue Value, bool IsSuccess, params Error[]? Error)
    : Response(IsSuccess, Error), ISingleResponse<TValue> where TValue : class, IBaseTableEntity
{
    [NotNull]
    public TValue Value { get; init; } = Value;
}

public record CollectionResponse<TValue>(ICollection<TValue> Values, bool IsSuccess, params Error[]? Error)
    : Response(IsSuccess, Error), ICollectionResponse<TValue> where TValue : class, IBaseTableEntity
{
    [NotNull]
    public ICollection<TValue> Values { get; init; } = Values;
}

public record CollectionResponseWithPagination<TValue>(
    ICollection<TValue> Values, bool IsSuccess,
    int TotalItems, int TotalPages, int CurrentPage, int PageSize)
    : Response(IsSuccess), ICollectionResponseWithPagination<TValue> where TValue : class, IBaseTableEntity
{
    [NotNull]
    public ICollection<TValue> Values { get; init; } = Values;
    public int TotalItems { get; init; } = TotalItems;
    public int TotalPages { get; init; } = TotalPages;
    public int CurrentPage { get; init; } = CurrentPage;
    public int PageSize { get; init; } = PageSize;
}

public record SuccessResponse() : Response(true);

public record FailureResponse(params Error[]? Error) : Response(false, Error);
