using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public class Appointment : BasePrimaryKeyEntity<int>
{
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Notes { get; set; }
    public decimal TotalPrice { get; set; }
    public AppointmentStatus Status { get; set; }

    public int ClientId { get; set; }
    public User Client { get; set; }
    
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public ICollection<ServiceAppointment> ServiceAppointments { get; set; } = [];
}
