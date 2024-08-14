using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IAddressService
    : IBasePrimaryKeyService<Address, int>, IHasOneForeignKeyService<Address, int>
{
    new Task<ISingleResponse<Address>> CreateAsync(IDtoRequest<Address> dtoRequest, int barberShopId);
}
