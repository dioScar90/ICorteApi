using System.Linq.Expressions;

namespace ICorteApi.Domain.Utils;

public record PaginationProperties<TEntity, TDtoResponse>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public Expression<Func<TEntity, bool>> Filter { get; init; }
    public Orderer OrderBy { get; init; }
    public Expression<Func<TEntity, TDtoResponse>> Select { get; init; }
    public Expression<Func<TEntity, object>>[] Includes { get; init; }

    public PaginationProperties(
        int? page, int? pageSize,
        Expression<Func<TEntity, bool>> filter,
        Orderer orderBy,
        Expression<Func<TEntity, TDtoResponse>> select,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var (realPage, realPpageSize) = GetSanitizedPagination(page, pageSize);

        Page = realPage;
        PageSize = realPpageSize;

        Filter = filter;
        OrderBy = orderBy;
        Includes = includes;
        Select = select;
    }

    private static (int, int) GetSanitizedPagination(int? page, int? pageSize)
    {
        int realPage = Math.Max(1, page ?? 1);
        int realPageSize = Math.Clamp(pageSize ?? 25, 1, 25);

        return (realPage, realPageSize);
    }
    
    public record Orderer(Expression<Func<TEntity, object>> KeySelector, bool IsDesc = false);
}
