namespace ICorteApi.Domain.Interfaces;

public interface ICollectionResponse<T> : IResponse where T : class, IBaseTableEntity
{
    ICollection<T>? Values { get; }
}

public interface ICollectionResponseWithPagination<T> : ICollectionResponse<T> where T : class, IBaseTableEntity
{
    int TotalItems { get; }
    int TotalPages { get; }
    int CurrentPage { get; }
    int PageSize { get; }
}
