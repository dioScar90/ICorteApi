namespace ICorteApi.Application.Services;

public class BarberScheduleService(IBarberScheduleRepository repository) : IBarberScheduleService
{
    private readonly IBarberScheduleRepository _repository = repository;

    private static int GetCorrectTakeNumber(int? take) => take is int and > 0 ? (int)take : 10;

    private static (DateOnly, DateOnly) GetFirstAndLastDatesOfWeek(DateOnly randomDate)
    {
        DateOnly firstDateThisWeek = randomDate.AddDays(-(int)randomDate.DayOfWeek);
        DateOnly lastDateThisWeek = firstDateThisWeek.AddDays(6);

        return (firstDateThisWeek, lastDateThisWeek);
    }

    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        if (serviceIds.Length == 0)
            return [];
        
        var (firstDateThisWeek, _) = GetFirstAndLastDatesOfWeek(date);
        return await _repository.GetAvailableSlotsAsync(barberShopId, date, firstDateThisWeek, serviceIds);
    }

    public async Task<TopBarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(DateOnly randomDate, int? take)
    {
        var (firstDateThisWeek, lastDateThisWeek) = GetFirstAndLastDatesOfWeek(randomDate);
        return await _repository.GetTopBarbersWithAvailabilityAsync(firstDateThisWeek, lastDateThisWeek, GetCorrectTakeNumber(take));
    }

    public async Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly randomDate)
    {
        var (firstDateThisWeek, _) = GetFirstAndLastDatesOfWeek(randomDate);
        return await _repository.GetAvailableDatesForBarberAsync(barberShopId, firstDateThisWeek);
    }
}
