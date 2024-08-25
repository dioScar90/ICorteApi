using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
    : IBaseRepository<BarberShop>
{
    Task<ICollectionResponse<BarberShop>> GetTop10BarbersWithAvailabilityAsync(int weekNumber);
}
