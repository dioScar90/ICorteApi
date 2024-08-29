using System.Linq.Expressions;

namespace ICorteApi.Domain.Base;

public record GetAllProperties<TEntity> : IGetAllProperties<TEntity>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public Expression<Func<TEntity, bool>> Filter { get; init; }
    public bool? IsDescending { get; init; }
    public Expression<Func<TEntity, object>>? OrderBy { get; init; }
    public Expression<Func<TEntity, object>>[] Includes { get; init; }

    public GetAllProperties(
        int? page, int? pageSize,
        Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var (realPage, realPpageSize) = GetSanitizedPagination(page, pageSize);

        Page = realPage;
        PageSize = realPpageSize;

        Filter = filter;
        Includes = includes;
    }

    public GetAllProperties(
        int? page, int? pageSize,
        Expression<Func<TEntity, bool>> filter,
        bool isDescending,
        Expression<Func<TEntity, object>> orderBy,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var (realPage, realPpageSize) = GetSanitizedPagination(page, pageSize);
        
        Page = realPage;
        PageSize = realPpageSize;

        Filter = filter;
        IsDescending = isDescending;
        OrderBy = orderBy;
        Includes = includes;
    }

    private static (int, int) GetSanitizedPagination(int? page, int? pageSize)
    {
        int realPage = Math.Max(1, page ?? 1);
        int realPageSize = Math.Clamp(pageSize ?? 25, 1, 25);

        return (realPage, realPageSize);
    }
}

public interface IGetAllProperties<TEntity>
{
    int Page { get; }
    int PageSize { get; }
    Expression<Func<TEntity, bool>> Filter { get; }
    Expression<Func<TEntity, object>>[] Includes { get; }
    bool? IsDescending { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
}

