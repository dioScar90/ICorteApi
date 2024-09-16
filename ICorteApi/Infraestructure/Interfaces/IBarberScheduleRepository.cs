namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberScheduleRepository : IRepository
{
    Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds);
    Task<TopBarberShop[]> GetTopBarbersWithAvailabilityAsync(DateOnly firstDateOfWeek, DateOnly lastDateOfWeek, int take);
    Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly firstDateOfWeek);
}
