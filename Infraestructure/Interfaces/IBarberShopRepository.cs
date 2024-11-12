namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
    : IBaseRepository<BarberShop>
{
    Task<BarberShop?> GetByIdAsync(int barberShopId);
}
