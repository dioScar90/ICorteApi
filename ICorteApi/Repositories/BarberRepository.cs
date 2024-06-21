using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICorteApi.Context;
using ICorteApi.Dtos;
using ICorteApi.Entities;

namespace ICorteApi.Repositories;

public class BarberRepository : IBaseRepository<Barber, BarberDtoRequest, BarberDtoResponse, ICorteContext>
{
    public Task<BarberDtoResponse> Create(BarberDtoRequest entity, ICorteContext context)
    {
        throw new NotImplementedException();
    }

    public Task<BarberDtoResponse> Delete(BarberDtoRequest entity, ICorteContext context)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id, ICorteContext context)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BarberDtoResponse>> GetAll(int offset, int limit, bool desc, ICorteContext context)
    {
        throw new NotImplementedException();
    }

    public Task<BarberDtoResponse> GetById(int id, ICorteContext context)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BarberDtoResponse> Update(BarberDtoRequest entity, ICorteContext context)
    {
        throw new NotImplementedException();
    }
}
