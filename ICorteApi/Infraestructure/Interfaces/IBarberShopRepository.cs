using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
    : IBaseRepository<BarberShop>
{
    Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek, int take);
}
