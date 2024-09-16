namespace ICorteApi.Application.Services;

public class BarberScheduleService(IBarberScheduleRepository repository) : IBarberScheduleService
{
    private readonly IBarberScheduleRepository _repository = repository;

    private static int GetCorrectTakeNumber(int? take) => take is int and > 0 ? (int)take : 10;

    private static (DateOnly, DateOnly) GetFirstAndLastDatesOfWeek(DateOnly randomDate)
    {
        DateOnly firstDateOfWeek = randomDate.AddDays(-(int)randomDate.DayOfWeek);
        DateOnly lastDateOfWeek = firstDateOfWeek.AddDays(6);

        return (firstDateOfWeek, lastDateOfWeek);
    }

    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        return await _repository.GetAvailableSlotsAsync(barberShopId, date, serviceIds);
    }

    public async Task<TopBarberShop[]> GetTopBarbersWithAvailabilityAsync(DateOnly randomDate, int? take)
    {
        var (firstDateOfWeek, lastDateOfWeek) = GetFirstAndLastDatesOfWeek(randomDate);
        return await _repository.GetTopBarbersWithAvailabilityAsync(firstDateOfWeek, lastDateOfWeek, GetCorrectTakeNumber(take));
    }

    public async Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly randomDate)
    {
        var (firstDateOfWeek, _) = GetFirstAndLastDatesOfWeek(randomDate);
        return await _repository.GetAvailableDatesForBarberAsync(barberShopId, firstDateOfWeek);
    }
}
