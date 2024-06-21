namespace BarberAppApi.Entities;

public class Barber : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public int UserId { get; set; }

    // Navigation Properties
    public User User { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Schedule> Schedules { get; set; } = [];
    public ICollection<Conversation> Conversations { get; set; } = [];
}

/*
CHAT GPT

Barber: Representa os barbeiros.

Id (int)
UserId (int) - Foreign Key
Specialty (string)
Navigation Properties: User, List<Appointment>, List<Schedule>
*/