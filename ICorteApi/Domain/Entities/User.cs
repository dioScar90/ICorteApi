using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class User : BasePrimaryKeyUserEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImageUrl { get; set; }
    
    public int? BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public BarberShop OwnedBarberShop { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
