namespace ICorteApi.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDate { get; set; }
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }
    public int ClientId { get; set; }
    public User Client { get; set; }
    public ICollection<Service> Services { get; set; }
}

/*
CHAT GPT

Appointment: Representa os agendamentos.

Id (int)
AppointmentDate (DateTime)
ServicesRequested (string)
BarberId (int) - Foreign Key
ClientId (int) - Foreign Key
PaymentId (int) - Foreign Key
CreatedAt (DateTime)
UpdatedAt (DateTime)
IsActive (bool)
Navigation Properties: Barber, Client, Payment, List<Service>
*/