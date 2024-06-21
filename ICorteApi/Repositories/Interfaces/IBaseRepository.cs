using ICorteApi.Context;
using ICorteApi.Dtos;
using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Repositories;

public interface IBaseRepository
{
    void Create<T, K>(T entity, K context) where T : BaseEntity where K : ICorteContext;
    void Update<T, K>(T entity, K context) where T : BaseEntity where K : ICorteContext;
    void Delete<T, K>(T entity, K context) where T : BaseEntity where K : ICorteContext;
}
