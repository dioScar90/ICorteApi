using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class BarberScheduleService(IBarberScheduleRepository repository) : IBarberScheduleService
{
    private readonly IBarberScheduleRepository _repository = repository;

    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        return await _repository.GetAvailableSlotsAsync(barberShopId, date, serviceIds);
    }
    
    public async Task<BarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(int weekNumber, int? take = 10)
    {
        int rightTake = take is int and > 0 ? (int)take : 10;
        var barberShops = await _repository.GetTopBarbersWithAvailabilityAsync(weekNumber, rightTake);
        return [..barberShops.Select(b => b.CreateDto())];
    }

    
    public async Task<DateOnly[]> GetAvailableDaysForBarberAsync(int barberShopId, DateOnly randomDate)
    {
        return await _repository.GetAvailableDaysForBarberAsync(barberShopId, randomDate);
    }
}
