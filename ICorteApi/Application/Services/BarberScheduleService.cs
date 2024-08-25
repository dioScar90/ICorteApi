using System.Globalization;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class BarberScheduleService(
        IAppointmentRepository appointmentRepository,
        IRecurringScheduleRepository recurringScheduleRepository,
        ISpecialScheduleRepository specialScheduleRepository)
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IRecurringScheduleRepository _recurringScheduleRepository = recurringScheduleRepository;
    private readonly ISpecialScheduleRepository _specialScheduleRepository = specialScheduleRepository;

    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, List<int> serviceIds)
    {
        // Recupera o horário de funcionamento regular e especial
        var recurringScheduleResponse = await _recurringScheduleRepository.GetByIdAsync(x => x.DayOfWeek == date.DayOfWeek && x.BarberShopId == barberShopId);
        var specialScheduleResponse = await _specialScheduleRepository.GetByIdAsync(x => x.Date == date && x.BarberShopId == barberShopId);

        if (!recurringScheduleResponse.IsSuccess)
            return [];

        if (!specialScheduleResponse.IsSuccess)
            return [];

        var recurringSchedule = recurringScheduleResponse.Value;
        var specialSchedule = specialScheduleResponse.Value;

        if (specialSchedule?.IsClosed ?? false)
            return []; // Barbeiro está fechado neste dia

        var openTime = specialSchedule?.OpenTime ?? recurringSchedule.OpenTime;
        var closeTime = specialSchedule?.CloseTime ?? recurringSchedule.CloseTime;

        // Calcula a duração total dos serviços
        var totalDuration = await CalculateTotalServiceDuration(serviceIds);

        // Recupera todos os agendamentos existentes para o dia especificado
        var appointments = await _appointmentRepository.GetAppointmentsByDateAsync(barberShopId, date);

        // Calcula os intervalos disponíveis
        var availableSlots = CalculateAvailableSlots(openTime, closeTime, appointments, totalDuration);

        return availableSlots;
    }

    private async Task<TimeSpan> CalculateTotalServiceDuration(int[] serviceIds)
    {
        // Implementação para somar a duração dos serviços
        return serviceIds.Sum();
    }

    private List<TimeOnly> CalculateAvailableSlots(TimeOnly openTime, TimeOnly closeTime, List<Appointment> appointments, TimeSpan serviceDuration)
    {
        var availableSlots = new List<TimeOnly>();

        // Lógica para calcular slots disponíveis entre os agendamentos existentes

        return availableSlots;
    }

    public async Task<List<BarberShop>> GetTop10BarbersWithAvailabilityAsync(int weekNumber)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        var top10Barbers = await _context.BarberShops
            .Where(b => b.Reports.Any(r => r.Rating > 0))
            .OrderByDescending(b => b.Reports.Average(r => r.Rating))
            .Select(b => new 
            {
                BarberShop = b,
                Availability = b.RecurringSchedules
                    .Where(s => s.OpenTime.HasValue && s.CloseTime.HasValue)
                    .Where(s => s.DayOfWeek >= startOfWeek.DayOfWeek && s.DayOfWeek <= endOfWeek.DayOfWeek)
            })
            .Where(b => b.Availability.Any())
            .Take(10)
            .Select(b => b.BarberShop)
            .ToListAsync();

        return top10Barbers;
    }

    public async Task<List<DayOfWeek>> GetAvailableDaysForBarberAsync(int barberShopId, int weekNumber)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        var availableDays = await _context.RecurringSchedules
            .Where(rs => rs.BarberShopId == barberShopId)
            .Where(rs => rs.OpenTime.HasValue && rs.CloseTime.HasValue)
            .Where(rs => rs.DayOfWeek >= startOfWeek.DayOfWeek && rs.DayOfWeek <= endOfWeek.DayOfWeek)
            .Select(rs => rs.DayOfWeek)
            .Distinct()
            .ToListAsync();

        return availableDays;
    }
}
