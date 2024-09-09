using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IAddressService : IService<Address>
{
    Task<AddressDtoResponse> CreateAsync(AddressDtoCreate dto, int barberShopId);
    Task<AddressDtoResponse> GetByIdAsync(int id, int barberShopId);
    Task<bool> UpdateAsync(AddressDtoUpdate dto, int id, int barberShopId);
    Task<bool> DeleteAsync(int id, int barberShopId);
}
