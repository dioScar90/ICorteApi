using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository repository)
    : BasePrimaryKeyService<Service, int>(repository), IServiceService
{
    public async Task<ISingleResponse<Service>> CreateAsync(IDtoRequest<Service> dto, int barberShopId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.BarberShopId = barberShopId;

        return await CreateByEntityAsync(entity);
    }
}
