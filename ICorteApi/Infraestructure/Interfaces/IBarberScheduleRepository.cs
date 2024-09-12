using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberScheduleRepository : IRepository
{
    Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds);
    Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(int weekNumber, int take);
    Task<DateOnly[]> GetAvailableDaysForBarberAsync(int barberShopId, DateOnly randomDate);
}
