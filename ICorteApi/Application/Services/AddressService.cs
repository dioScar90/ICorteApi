using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class AddressService(IAddressRepository repository)
    : BasePrimaryKeyService<Address, int>(repository), IAddressService
{
    public async Task<ISingleResponse<Address>> CreateAsync(IDtoRequest<Address> dto, int barberShopId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.BarberShopId = barberShopId;

        return await CreateByEntityAsync(entity);
    }
}
