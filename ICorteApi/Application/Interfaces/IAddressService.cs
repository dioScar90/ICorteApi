using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IAddressService : IService<Address>
{
    Task<ISingleResponse<Address>> CreateAsync(IDtoRequest<Address> dtoRequest, int barberShopId);
    Task<ISingleResponse<Address>> GetByIdAsync(int id, int barberShopId);
    Task<IResponse> UpdateAsync(IDtoRequest<Address> dtoRequest, int id, int barberShopId);
    Task<IResponse> DeleteAsync(int id, int barberShopId);
}
