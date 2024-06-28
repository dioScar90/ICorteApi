using ICorteApi.Enums;

namespace ICorteApi.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDate { get; set; }
    public string? Notes { get; set; }
    public decimal TotalPrice { get; set; }
    public AppointmentStatus Status { get; set; }
    
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }
    
    public int ClientId { get; set; }
    public Person Client { get; set; }
    
    public IEnumerable<AppointmentService> AppointmentServices { get; set; }
}

// Alternative for using RecurringSchedule:
// public class Appointment
// {
//     public int Id { get; set; }
//     public DateTime AppointmentDate { get; set; }
//     public TimeSpan StartTime { get; set; }
//     public TimeSpan EndTime { get; set; }
//     public int BarberId { get; set; }
//     public Person Client { get; set; }
//     public Barber Barber { get; set; }
//     public IEnumerable<Service> Services { get; set; }
// }
