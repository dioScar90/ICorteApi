using ICorteApi.Entities;
using ICorteApi.Dtos;
using ICorteApi.Context;

namespace ICorteApi.Repositories.Interfaces;

public interface IBarberRepository : IBaseRepository
{
    IQueryable<T> GetAll<T>(int page, int perPage) where T : BaseEntity;
    Task<T?> GetById<T>(int id) where T : BaseEntity;
}
