namespace BarberAppApi.Entities;

public class Client : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime LastVisitDate { get; set; }
    public int UserId { get; set; }

    // Navigation Properties
    public User User { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Conversation> Conversations { get; set; } = [];
}

/*
CHAT GPT

Client: Representa os clientes.

Id (int)
UserId (int) - Foreign Key
Navigation Properties: User, List<Appointment>, Address
*/