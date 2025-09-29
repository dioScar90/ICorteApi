using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ICorteApi.Domain.Entities;

public sealed class Service : BaseEntity<Service>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Duration { get; private set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; init; }

    public ICollection<Appointment> Appointments { get; init; } = [];

    private Service() { }
    
    public Service(ServiceDtoRequest dto, int? barberShopId = null)
    {
        Name = dto.Name;
        Description = GetValidStringOrNull(dto.Description);
        Price = dto.Price;
        Duration = dto.Duration;

        BarberShopId = barberShopId ?? default;
    }
    
    public void UpdateEntity(ServiceDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        Name = dto.Name;
        Description = GetValidStringOrNull(dto.Description);
        Price = dto.Price;
        Duration = dto.Duration;

        UpdatedAt = utcNow;
    }

    public ServiceDtoResponse CreateDto() => new(
        Id,
        BarberShopId,
        default,
        Name,
        Description,
        Price,
        Duration
    );
}
