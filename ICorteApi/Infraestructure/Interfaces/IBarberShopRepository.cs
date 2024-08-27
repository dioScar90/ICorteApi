using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
    : IBaseRepository<BarberShop>
{
    Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek, int take);
}
