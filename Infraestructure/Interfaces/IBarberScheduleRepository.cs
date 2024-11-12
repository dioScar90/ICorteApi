namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberScheduleRepository : IRepository
{
    Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly dateToSearchForSlots, DateOnly firstDateThisWeek, int[] serviceIds);
    Task<TopBarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(DateOnly firstDateThisWeek, DateOnly lastDateThisWeek, int take);
    Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly firstDateThisWeek);
}
