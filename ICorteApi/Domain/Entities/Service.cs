using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Service : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public ICollection<AppointmentService> AppointmentServices { get; set; } = [];
}
