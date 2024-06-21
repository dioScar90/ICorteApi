using ICorteApi.Context;
using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Repositories;

public abstract class BaseRepository : IBaseRepository
{
    public async void Create<T, K>(T entity, K context)
        where T : BaseEntity
        where K : DbContext
    {
        context.AddAsync(entity);
    }

    public void Delete<T, K>(T entity, K context)
        where T : BaseEntity
        where K : DbContext
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAll<T, K>(int offset, int limit, bool desc, K context)
        where T : BaseEntity
        where K : DbContext
    {
        throw new NotImplementedException();
    }

    public Task<T> GetById<T, K>(int id, K context)
        where T : BaseEntity
        where K : DbContext
    {
        return await context.Get
    }

    public void Update<T, K>(T entity, K context)
        where T : BaseEntity
        where K : DbContext
    {
        context.Update(entity);
    }
}
