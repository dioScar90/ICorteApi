using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class AppointmentService : BaseHardCrudEntity
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }
}

/*
    INCLUIR ESSE MÉTODO NO SERVICE DE (AppointmentService) AppointmentServiceService,
    OU TALVEZ NO SERVICE DE Appointment (AppointmentService)
*/

// public bool IsTimeSlotAvailable(int barberShopId, DateOnly desiredDate, TimeOnly desiredStartTime, TimeSpan duration)
// {
//     var desiredEndTime = desiredStartTime.Add(duration);

//     // Verifique se há um horário especial para a data específica
//     var specialSchedule = _context.SpecialSchedules
//         .FirstOrDefault(ss => ss.BarberShopId == barberShopId && ss.Date == desiredDate);

//     if (specialSchedule != null)
//     {
//         if (specialSchedule.IsClosed ||
//             desiredStartTime < specialSchedule.OpenTime || desiredEndTime > specialSchedule.CloseTime)
//         {
//             return false;
//         }
//     }
//     else
//     {
//         // Caso não haja um horário especial, verifique o horário recorrente
//         var dayOfWeek = desiredDate.DayOfWeek;
//         var recurringSchedule = _context.RecurringSchedules
//             .FirstOrDefault(rs => rs.BarberShopId == barberShopId && rs.DayOfWeek == dayOfWeek);

//         if (recurringSchedule == null ||
//             desiredStartTime < recurringSchedule.OpenTime || desiredEndTime > recurringSchedule.CloseTime)
//         {
//             return false;
//         }
//     }

//     // Verifique conflitos de agendamentos
//     var conflictingAppointments = _context.Appointments
//         .Any(a => a.BarberShopId == barberShopId &&
//                   a.Date == desiredDate &&
//                   a.StartTime < desiredEndTime &&
//                   a.EndTime > desiredStartTime);

//     return !conflictingAppointments;
// }
