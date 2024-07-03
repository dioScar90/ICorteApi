using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class AppointmentService : IBaseTableEntity
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }
}
