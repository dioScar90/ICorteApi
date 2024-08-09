using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class User : BasePrimaryKeyUserEntity<int>
{
    [PreventNullAfterSet]
    public string? FirstName { get; set; }
    [PreventNullAfterSet]
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    
    public int? BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public BarberShop OwnedBarberShop { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
