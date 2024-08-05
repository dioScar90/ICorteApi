using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository serviceRepository)
    : BasePrimaryKeyService<Service, int>(serviceRepository), IServiceService
{
}
