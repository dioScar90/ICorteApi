using System.ComponentModel;
using System.Text.Json.Serialization;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Service : BasePrimaryKeyEntity<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Duration { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public ICollection<AppointmentService> AppointmentServices { get; set; } = [];
}
