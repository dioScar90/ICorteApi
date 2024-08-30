using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IAddressService : IService<Address>
{
    Task<Address?> CreateAsync(AddressDtoCreate dto, int barberShopId);
    Task<Address?> GetByIdAsync(int id, int barberShopId);
    Task<bool> UpdateAsync(AddressDtoUpdate dto, int id, int barberShopId);
    Task<bool> DeleteAsync(int id, int barberShopId);
}
