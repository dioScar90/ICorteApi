namespace ICorteApi.Application.Interfaces;

public interface IBarberScheduleService : IService
{
    Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds);
    Task<TopBarberShop[]> GetTopBarbersWithAvailabilityAsync(DateOnly dateOfWeek, int? take);
    Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly dateOfWeek);
}
