using System.Linq.Expressions;

namespace ICorteApi.Domain.Base;

public record PaginationProperties<TEntity> : IPaginationProperties<TEntity>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public Expression<Func<TEntity, bool>> Filter { get; init; }
    public bool IsDescending { get; init; }
    public Expression<Func<TEntity, object>> OrderBy { get; init; }
    public Expression<Func<TEntity, object>>[] Includes { get; init; }
    
    public PaginationProperties(
        int? page, int? pageSize,
        Expression<Func<TEntity, bool>> filter,
        OrderByRec orderByRec,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var (realPage, realPpageSize) = GetSanitizedPagination(page, pageSize);

        Page = realPage;
        PageSize = realPpageSize;

        Filter = filter;
        IsDescending = orderByRec.IsDescending is true;
        OrderBy = orderByRec.OrderBy;
        Includes = includes;
    }

    private static (int, int) GetSanitizedPagination(int? page, int? pageSize)
    {
        int realPage = Math.Max(1, page ?? 1);
        int realPageSize = Math.Clamp(pageSize ?? 25, 1, 25);

        return (realPage, realPageSize);
    }

    public record OrderByRec(
        Expression<Func<TEntity, object>> OrderBy,
        bool? IsDescending = null
    );
}

public interface IPaginationProperties<TEntity>
{
    int Page { get; }
    int PageSize { get; }
    Expression<Func<TEntity, bool>> Filter { get; }
    Expression<Func<TEntity, object>>[] Includes { get; }
    Expression<Func<TEntity, object>> OrderBy { get; }
    bool IsDescending { get; }
}

