using ICorteApi.Enums;

namespace ICorteApi.Entities;

public class Service : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    public int BarberShopId { get; set; }
    public User BarberShop { get; set; }

    public IEnumerable<AppointmentService> AppointmentServices { get; set; }
}
