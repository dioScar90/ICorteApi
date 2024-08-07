using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository repository)
    : BasePrimaryKeyService<Service, int>(repository), IServiceService
{
}
