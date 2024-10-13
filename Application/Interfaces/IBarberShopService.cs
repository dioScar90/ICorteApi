namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService : IService<BarberShop>
{
    Task<BarberShopDtoResponse> CreateAsync(BarberShopDtoCreate dto, int ownerId);
    Task<BarberShopDtoResponse> GetByIdAsync(int id);
    Task<bool> UpdateAsync(BarberShopDtoUpdate dto, int id, int ownerId);
    Task<bool> DeleteAsync(int id, int ownerId);
}