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
