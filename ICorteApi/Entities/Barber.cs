namespace ICorteApi.Entities;

public class Barber : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public int UserId { get; set; }

    // Navigation Properties
    public virtual User User { get; set; }
    // public virtual ICollection<Appointment> Appointments { get; set; } = [];
    // public virtual ICollection<Schedule> Schedules { get; set; } = [];
    // public virtual ICollection<Conversation> Conversations { get; set; } = [];
}

/*
CHAT GPT

Barber: Representa os barbeiros.

Id (int)
UserId (int) - Foreign Key
Specialty (string)
Navigation Properties: User, List<Appointment>, List<Schedule>
*/