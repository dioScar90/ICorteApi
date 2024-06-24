using ICorteApi.Context;
using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Repositories;

public abstract class BaseRepository(ICorteContext context) : IBaseRepository
{
    private readonly ICorteContext _context = context;
    public async void Create<T>(T entity) where T : BaseEntity
    {
        context.Add(entity);
    }

    public async void Delete<T>(T entity) where T : BaseEntity
    {
        context.Remove(entity);
    }
    
    public void Update<T>(T entity) where T : BaseEntity
    {
        context.Update(entity);
    }
    
    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() > 0;
}
