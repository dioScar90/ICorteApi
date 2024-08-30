using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService : IService<BarberShop>
{
    Task<BarberShop?> CreateAsync(BarberShopDtoRequest dto, int ownerId);
    Task<BarberShop?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(BarberShopDtoRequest dto, int id, int ownerId);
    Task<bool> DeleteAsync(int id, int ownerId);
}
