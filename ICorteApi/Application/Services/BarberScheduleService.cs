using System.Globalization;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class BarberScheduleService(
    IAppointmentRepository appointmentRepository,
    IBarberShopRepository barberShopRepository,
    IRecurringScheduleRepository recurringScheduleRepository,
    ISpecialScheduleRepository specialScheduleRepository)
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IBarberShopRepository _barberShopRepository = barberShopRepository;
    private readonly IRecurringScheduleRepository _recurringScheduleRepository = recurringScheduleRepository;
    private readonly ISpecialScheduleRepository _specialScheduleRepository = specialScheduleRepository;

    // public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, List<int> serviceIds)
    // {
    //     // Recupera o horário de funcionamento regular e especial
    //     var recurringScheduleResponse = await _recurringScheduleRepository.GetByIdAsync(x => x.DayOfWeek == date.DayOfWeek && x.BarberShopId == barberShopId);
    //     var specialScheduleResponse = await _specialScheduleRepository.GetByIdAsync(x => x.Date == date && x.BarberShopId == barberShopId);

    //     if (!recurringScheduleResponse.IsSuccess)
    //         return [];

    //     if (!specialScheduleResponse.IsSuccess)
    //         return [];

    //     var recurringSchedule = recurringScheduleResponse.Value;
    //     var specialSchedule = specialScheduleResponse.Value;

    //     if (specialSchedule?.IsClosed ?? false)
    //         return []; // Barbeiro está fechado neste dia

    //     var openTime = specialSchedule?.OpenTime ?? recurringSchedule.OpenTime;
    //     var closeTime = specialSchedule?.CloseTime ?? recurringSchedule.CloseTime;

    //     // Calcula a duração total dos serviços
    //     var totalDuration = await CalculateTotalServiceDuration(serviceIds);

    //     // Recupera todos os agendamentos existentes para o dia especificado
    //     var appointments = await _appointmentRepository.GetAppointmentsByDateAsync(barberShopId, date);

    //     // Calcula os intervalos disponíveis
    //     var availableSlots = CalculateAvailableSlots(openTime, closeTime, appointments, totalDuration);

    //     return availableSlots;
    // }

    // private async Task<TimeSpan> CalculateTotalServiceDuration(int[] serviceIds)
    // {
    //     // Implementação para somar a duração dos serviços
    //     return serviceIds.Sum();
    // }

    // private List<TimeOnly> CalculateAvailableSlots(TimeOnly openTime, TimeOnly closeTime, List<Appointment> appointments, TimeSpan serviceDuration)
    // {
    //     var availableSlots = new List<TimeOnly>();

    //     // Lógica para calcular slots disponíveis entre os agendamentos existentes

    //     return availableSlots;
    // }

    public async Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(int weekNumber, int take = 10)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        return await _barberShopRepository.GetTopBarbersWithAvailabilityAsync(startOfWeek.DayOfWeek, endOfWeek.DayOfWeek, take);
    }

    public async Task<DayOfWeek[]> GetAvailableDaysForBarberAsync(int barberShopId, int weekNumber)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        return await _recurringScheduleRepository.GetAvailableDaysForBarberAsync(barberShopId, startOfWeek.DayOfWeek, endOfWeek.DayOfWeek);
    }
}
