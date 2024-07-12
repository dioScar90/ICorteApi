using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

// Pessoa
public class Person : BaseCrudEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? LastVisitDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    
    public int? BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
    
    public BarberShop OwnedBarberShop { get; set; }
    // public ICollection<Schedule> Schedules { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    // public ICollection<Report> Reports { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<PersonConversation> PersonConversations { get; set; } = [];
}
