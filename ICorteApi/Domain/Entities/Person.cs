using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class Person : BaseCrudEntity, IPrimaryKeyEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int? BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public BarberShop OwnedBarberShop { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = [];
    
    public int Key => UserId;
}
