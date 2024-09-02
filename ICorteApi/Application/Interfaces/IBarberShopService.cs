using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService : IService<BarberShop>
{
    Task<BarberShop?> CreateAsync(BarberShopDtoCreate dto, int ownerId);
    Task<BarberShop?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(BarberShopDtoUpdate dto, int id, int ownerId);
    Task<bool> DeleteAsync(int id, int ownerId);
}
