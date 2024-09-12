// using System.Globalization;
// using ICorteApi.Domain.Entities;
// using ICorteApi.Domain.Interfaces;
// using ICorteApi.Infraestructure.Interfaces;

// namespace ICorteApi.Application.Services;

// public class BarberScheduleService(
//     IBarberShopRepository barberShopRep,
//     IAppointmentRepository appointmentRep,
//     IServiceRepository serviceRep,
//     IRecurringScheduleRepository recurringScheduleRep,
//     ISpecialScheduleRepository specialScheduleRep)
// {
//     private readonly IBarberShopRepository _barberShopRep = barberShopRep;
//     private readonly IServiceRepository _serviceRep = serviceRep;
//     private readonly IAppointmentRepository _appointmentRep = appointmentRep;
//     private readonly IRecurringScheduleRepository _recurringScheduleRep = recurringScheduleRep;
//     private readonly ISpecialScheduleRepository _specialScheduleRep = specialScheduleRep;

//     public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
//     {
//         // Recupera o horário de funcionamento regular e especial
//         var recurringSchedule = await _recurringScheduleRep.GetByIdAsync(date.DayOfWeek, barberShopId);
//         var specialSchedule = await _specialScheduleRep.GetByIdAsync(date, barberShopId);

//         if (recurringSchedule is null || specialSchedule is null)
//             return [];
            
//         if (specialSchedule.IsClosed)
//             return []; // Barbeiro está fechado neste dia

//         var openTime = specialSchedule?.OpenTime ?? recurringSchedule.OpenTime;
//         var closeTime = specialSchedule?.CloseTime ?? recurringSchedule.CloseTime;

//         // Calcula a duração total dos serviços
//         var totalDuration = await CalculateTotalServiceDuration(serviceIds);

//         // Recupera todos os agendamentos existentes para o dia especificado
//         var appointments = await _appointmentRep.GetAppointmentsByDateAsync(barberShopId, date);

//         // Calcula os intervalos disponíveis
//         return CalculateAvailableSlots(openTime, closeTime, appointments, totalDuration);
//     }

//     private async Task<TimeSpan> CalculateTotalServiceDuration(int[] serviceIds)
//     {
//         // Implementação para somar a duração dos serviços
//         // return serviceIds.Sum();
//         return await _serviceRep.CalculateTotalServiceDuration(serviceIds);
//     }

//     private List<TimeOnly> CalculateAvailableSlots(TimeOnly openTime, TimeOnly closeTime, List<Appointment> appointments, TimeSpan serviceDuration)
//     {
//         var availableSlots = new List<TimeOnly>();

//         // Lógica para calcular slots disponíveis entre os agendamentos existentes

//         return availableSlots;
//     }

//     public async Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(int weekNumber, int take = 10)
//     {
//         var currentYear = DateTime.UtcNow.Year;
//         var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
//         var endOfWeek = startOfWeek.AddDays(7);

//         return await _barberShopRep.GetTopBarbersWithAvailabilityAsync(startOfWeek.DayOfWeek, endOfWeek.DayOfWeek, take);
//     }

//     public async Task<DayOfWeek[]> GetAvailableDaysForBarberAsync(int barberShopId, int weekNumber)
//     {
//         var currentYear = DateTime.UtcNow.Year;
//         var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
//         var endOfWeek = startOfWeek.AddDays(7);

//         return await _recurringScheduleRep.GetAvailableDaysForBarberAsync(barberShopId, startOfWeek.DayOfWeek, endOfWeek.DayOfWeek);
//     }
// }
