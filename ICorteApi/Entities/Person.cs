namespace ICorteApi.Entities;

public class Person : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? LastVisitDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    
    public int? BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
    
    public BarberShop OwnedBarberShop { get; set; }
    public IEnumerable<Appointment> Appointments { get; set; } = [];
    public IEnumerable<Report> Reports { get; set; } = [];
    public IEnumerable<Message> Messages { get; set; } = [];
    public IEnumerable<PersonConversation> PersonConversations { get; set; } = [];
}
