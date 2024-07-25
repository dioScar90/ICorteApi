using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class ServiceRepository(AppDbContext context)
    : BasePrimaryKeyRepository<Service, int>(context), IServiceRepository
{
}
