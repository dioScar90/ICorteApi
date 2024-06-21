using ICorteApi.Entities;
using ICorteApi.Dtos;
using ICorteApi.Context;

namespace ICorteApi.Repositories.Interfaces;

public interface IBarberRepository : IBaseRepository
{
    Task<IEnumerable<T>> GetAll<T, K>(K context, int? offset, int? limit, bool? desc) where T : BaseEntity where K : ICorteContext;
    Task<T> GetById<T, K>(int id, K context) where T : BaseEntity where K : ICorteContext;
}
