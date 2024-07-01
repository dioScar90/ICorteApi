using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Report : BaseEntity
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Rating { get; set; }
    
    public int ClientId { get; set; }
    public Person Client { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
}
