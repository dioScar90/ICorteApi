namespace ICorteApi.Entities;

public class Person : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? LastVisitDate { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
    
    public IEnumerable<Appointment> Appointments { get; set; } = [];
    public IEnumerable<Conversation> Conversations { get; set; } = [];
    public IEnumerable<Service> Services { get; set; } = [];
    public IEnumerable<Report> Reports { get; set; } = [];
    public IEnumerable<Address> Addresses { get; set; } = [];
}
