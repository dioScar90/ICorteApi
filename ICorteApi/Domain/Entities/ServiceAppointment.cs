using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class ServiceAppointment : CompositeKeyEntity<ServiceAppointment, int, int>
{
    public int AppointmentId { get => Id1; init => Id1 = value; }
    public Appointment Appointment { get; init; }

    public int ServiceId { get => Id2; init => Id2 = value; }
    public Service Service { get; init; }

    private ServiceAppointment() {}

    public ServiceAppointment(ServiceAppointmentDtoRequest dto)
    {
        if (dto is not { AppointmentId: int, ServiceId: int })
            throw new Exception("AppointmentId and ServiceId required");

        AppointmentId = dto.AppointmentId;
        ServiceId = dto.ServiceId;
    }
    
    private void UpdateByServiceAppointmentDto(ServiceAppointmentDtoRequest dto, DateTime? utcNow)
    {
        // utcNow ??= DateTime.UtcNow;

        // OpenTime = dto.OpenTime;
        // CloseTime = dto.CloseTime;

        // UpdatedAt = utcNow;
    }
    
    public override void UpdateEntityByDto(IDtoRequest<ServiceAppointment> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case ServiceAppointmentDtoRequest dto:
                UpdateByServiceAppointmentDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }
    
    public override ServiceAppointmentDtoResponse CreateDto() =>
        new(
            AppointmentId,
            ServiceId
        );
}

/*
    INCLUIR ESSE MÉTODO NO SERVICE DE (ServiceAppointment) ServiceAppointmentService,
    OU TALVEZ NO SERVICE DE Appointment (ServiceAppointment)
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
