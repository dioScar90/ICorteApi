using System.ComponentModel;
using System.Text.Json.Serialization;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Entities;

public sealed class Service : BaseEntity<Service, ServiceDto>
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
    
    public Service(ServiceDto dto, int? barberShopId = null)
    {
        dto.ThrowExceptionIfInvalid(new ServiceDtoValidator(), new ServiceErrors());

        Name = dto.Name;
        Description = GetValidStringOrNull(dto.Description);
        Price = dto.Price;
        Duration = dto.Duration;

        BarberShopId = barberShopId ?? default;
    }
    
    public override void UpdateEntityByDto(ServiceDto dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        Name = dto.Name;
        Description = GetValidStringOrNull(dto.Description);
        Price = dto.Price;
        Duration = dto.Duration;

        UpdatedAt = utcNow;
    }

    public override ServiceDto CreateDto() => new(
        Id,
        BarberShopId,
        default,
        Name,
        Description,
        Price,
        Duration
    );
}
