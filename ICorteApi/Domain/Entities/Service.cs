using System.ComponentModel;
using System.Text.Json.Serialization;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Service : BasePrimaryKeyEntity<Service, int>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Duration { get; private set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; init; }

    public ICollection<ServiceAppointment> ServiceAppointments { get; init; } = [];

    private Service() {}

    public Service(ServiceDtoRequest dto, int? barberShopId = null)
    {
        Name = dto.Name;
        Description = dto.Description;
        Price = dto.Price;

        BarberShopId = barberShopId ?? default;
    }

    private void UpdateByServiceDto(ServiceDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Name = dto.Name;
        Description = dto.Description;
        Price = dto.Price;
        
        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Service> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case ServiceDtoRequest dto:
                UpdateByServiceDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override ServiceDtoResponse CreateDto() =>
        new(
            Id,
            Name,
            Description ?? default,
            Price,
            Duration
        );
}
