using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AddressService(IAddressRepository repository)
    : BasePrimaryKeyService<Address, int>(repository), IAddressService
{
    public async Task<ISingleResponse<Address>> CreateAsync(IDtoRequest<Address> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not AddressDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Address(dto, barberShopId);
        return await CreateByEntityAsync(entity);
    }
}
