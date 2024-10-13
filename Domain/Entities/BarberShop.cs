using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class BarberShop : BaseEntity<BarberShop>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string ComercialNumber { get; private set; }
    public string ComercialEmail { get; private set; }
    public string? ImageUrl { get; private set; }
    public float Rating { get; private set; }

    public int OwnerId { get; init; }
    public User Owner { get; init; }

    public Address Address { get; init; }
    public ICollection<RecurringSchedule> RecurringSchedules { get; init; } = [];
    public ICollection<SpecialSchedule> SpecialSchedules { get; init; } = [];
    public ICollection<Service> Services { get; init; } = [];
    public ICollection<Appointment> Appointments { get; init; } = [];
    public ICollection<Report> Reports { get; init; } = [];

    private BarberShop() { }

    public BarberShop(BarberShopDtoCreate dto, int? ownerId = null)
    {
        Name = dto.Name;
        Description = dto.Description ?? default;
        ComercialNumber = dto.ComercialNumber;
        ComercialEmail = dto.ComercialEmail;

        if (dto.Address is not null)
            Address = new(dto.Address);

        if (dto.RecurringSchedules is not null)
            RecurringSchedules = dto.RecurringSchedules.Select(rs => new RecurringSchedule(rs)).ToArray();

        if (dto.SpecialSchedules is not null)
            SpecialSchedules = dto.SpecialSchedules.Select(ss => new SpecialSchedule(ss)).ToArray();

        if (dto.Services is not null)
            Services = dto.Services.Select(s => new Service(s)).ToArray();

        OwnerId = ownerId ?? default;
    }

    public void UpdateRating(float rating) => Rating = rating;

    private void UpdateByBarberShopDto(BarberShopDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Name = dto.Name;
        Description = dto.Description ?? null;
        ComercialNumber = dto.ComercialNumber;
        ComercialEmail = dto.ComercialEmail;

        if (dto.Address is not null)
            Address?.UpdateEntityByDto(dto.Address, utcNow);

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<BarberShop> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case BarberShopDtoUpdate dto:
                UpdateByBarberShopDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }
    
    public override BarberShopDtoResponse CreateDto() =>
        new(
            Id,
            OwnerId,
            Name,
            Description,
            ComercialNumber,
            ComercialEmail,
            Address?.CreateDto(),
            RecurringSchedules  is null ? [] : [..RecurringSchedules.Select(b => b.CreateDto())],
            SpecialSchedules    is null ? [] : [..SpecialSchedules.Select(b => b.CreateDto())],
            Services            is null ? [] : [..Services.Select(b => b.CreateDto())],
            Reports             is null ? [] : [..Reports.Select(b => b.CreateDto())]
        );
}
