using ICorteApi.Context;
using ICorteApi.Dtos;
using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Repositories;

public interface IBaseRepository
{
    void Create<T>(T entity) where T : BaseEntity;
    void Update<T>(T entity) where T : BaseEntity;
    void Delete<T>(T entity) where T : BaseEntity;
    Task<bool> SaveChangesAsync();
}
