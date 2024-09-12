using ICorteApi.Application.Dtos;

namespace ICorteApi.Application.Interfaces;

public interface IBarberScheduleService : IService
{
    Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds);
    Task<BarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(int weekNumber, int? take = 10);
    Task<DateOnly[]> GetAvailableDaysForBarberAsync(int barberShopId, DateOnly randomDate);
}
