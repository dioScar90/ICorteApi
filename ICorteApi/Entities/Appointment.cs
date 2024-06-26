namespace ICorteApi.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDate { get; set; }
    
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }

    // public IEnumerable<Service> Services { get; set; }
}
