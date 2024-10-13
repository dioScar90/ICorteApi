using System.ComponentModel;
using System.Text.Json.Serialization;
using ICorteApi.Domain.Base;

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
    
    public Service(ServiceDtoCreate dto, int? barberShopId = null)
    {
        Name = dto.Name;
        Description = GetValidStringOrNull(dto.Description);
        Price = dto.Price;
        Duration = dto.Duration;

        BarberShopId = barberShopId ?? default;
    }
    
    private void UpdateByServiceDto(ServiceDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Name = dto.Name;
        Description = GetValidStringOrNull(dto.Description);
        Price = dto.Price;
        Duration = dto.Duration;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Service> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case ServiceDtoUpdate dto:
                UpdateByServiceDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override ServiceDtoResponse CreateDto() =>
        new(
            Id,
            BarberShopId,
            Name,
            Description,
            Price,
            Duration
        );
}
