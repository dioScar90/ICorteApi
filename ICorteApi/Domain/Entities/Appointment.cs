using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Appointment : BaseEntity<Appointment>
{
    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeSpan TotalDuration { get; private set; }
    public string? Notes { get; private set; }
    public PaymentType PaymentType { get; private set; }
    public decimal TotalPrice { get; private set; }
    public AppointmentStatus Status { get; private set; }

    public int ClientId { get; init; }
    public User Client { get; set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; set; }

    public ICollection<Message> Messages { get; init; } = [];
    public ICollection<Service> Services { get; init; } = [];

    private Appointment() { }

    public Appointment(AppointmentDtoCreate dto, Service[] services, int clientId)
    {
        Date = dto.Date;
        StartTime = dto.StartTime;
        Notes = dto.Notes;
        PaymentType = dto.PaymentType;
        
        Services = services;
        UpdatePriceAndDuration();

        Status = AppointmentStatus.Pending;

        ClientId = clientId;
        BarberShopId = services[0].BarberShopId;
    }

    public void AddServices(Service[] servicesToAdd)
    {
        foreach (var toAdd in servicesToAdd)
            Services.Add(toAdd);
    }
    
    public void RemoveServicesByIds(int[] serviceIdsToRemove)
    {
        HashSet<int> idsToRemove = [..serviceIdsToRemove];
        var servicesToRemove = Services.Where(t => idsToRemove.Contains(t.Id)).ToArray();

        foreach (var toRemove in servicesToRemove)
            Services.Remove(toRemove);
    }

    private void UpdateTotalDuration() => TotalDuration = Services.Aggregate(TimeSpan.Zero, (acc, curr) => acc.Add(curr.Duration));
    private void UpdateTotalPrice() => TotalPrice = Services.Aggregate(decimal.Zero, (acc, curr) => acc + curr.Price);

    public void UpdatePriceAndDuration()
    {
        UpdateTotalDuration();
        UpdateTotalPrice();
    }
    
    private void UpdateByAppointmentDto(AppointmentDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Date = dto.Date;
        StartTime = dto.StartTime;
        Notes = dto.Notes;
        PaymentType = dto.PaymentType;

        UpdatePriceAndDuration();
        
        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Appointment> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case AppointmentDtoUpdate dto:
                UpdateByAppointmentDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    private ServiceDtoResponse[] GetServicesIntoDto() => [..Services.Select(s => s.CreateDto())];

    public override AppointmentDtoResponse CreateDto() =>
        new(
            Id,
            ClientId,
            BarberShopId,
            Date,
            StartTime,
            TotalDuration,
            Notes,
            PaymentType,
            TotalPrice,
            GetServicesIntoDto(),
            Status
        );
}

public enum PaymentType
{
    Card,
    Cash,
    Transfer,
    Other
}

public enum AppointmentStatus
{
    Pending,
    Completed
}
