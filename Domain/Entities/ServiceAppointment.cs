namespace ICorteApi.Domain.Entities;

public sealed class ServiceAppointment : BaseEntity<Appointment>
{
    public int AppointmentId { get; init; }
    public Appointment Appointment { get; set; }

    public int ServiceId { get; init; }
    public Service Service { get; set; }
    
    private ServiceAppointment() { }
    
    // public ServiceAppointment(AppointmentDtoRequest dto, Service[] services)
    // {
    //     Date = dto.Date;
    //     StartTime = dto.StartTime;
    //     Notes = dto.Notes;
    //     PaymentType = dto.PaymentType;
        
    //     Services = services;

    //     UpdatePriceAndDuration();

    //     Status = AppointmentStatus.Pending;

    //     ClientId = dto.ClientId;
    //     BarberShopId = services[0].BarberShopId;
    // }

    // public void AddServices(Service[] servicesToAdd)
    // {
    //     foreach (var toAdd in servicesToAdd)
    //         Services.Add(toAdd);
    // }
    
    // public void RemoveServicesByIds(int[] serviceIdsToRemove)
    // {
    //     HashSet<int> idsToRemove = [..serviceIdsToRemove];
    //     var servicesToRemove = Services.Where(t => idsToRemove.Contains(t.Id)).ToArray();

    //     foreach (var toRemove in servicesToRemove)
    //         Services.Remove(toRemove);
    // }

    // private void UpdateTotalDuration() => TotalDuration = Services.Aggregate(TimeSpan.Zero, (acc, curr) => acc.Add(curr.Duration));
    // private void UpdateTotalPrice() => TotalPrice = Services.Aggregate(decimal.Zero, (acc, curr) => acc + curr.Price);

    // public void UpdatePriceAndDuration()
    // {
    //     UpdateTotalDuration();
    //     UpdateTotalPrice();
    // }
    
    // public void UpdateEntity(AppointmentDtoRequest dto, DateTime? utcNow = null)
    // {
    //     utcNow ??= DateTime.UtcNow;

    //     Date = dto.Date;
    //     StartTime = dto.StartTime;
    //     Notes = dto.Notes;
    //     PaymentType = dto.PaymentType;

    //     UpdatePriceAndDuration();

    //     UpdatedAt = utcNow;
    // }
    
    // public void UpdateEntity(AppointmentPaymentTypeDtoUpdateRequest dto, DateTime? utcNow = null)
    // {
    //     utcNow ??= DateTime.UtcNow;
        
    //     PaymentType = dto.PaymentType;
        
    //     UpdatedAt = utcNow;
    // }
    
    // private ServiceDtoResponse[] GetServicesIntoDto() => [.. Services.Select(s => s.CreateDto())];

    // public AppointmentDtoResponse CreateDto() => new(
    //     Id,
    //     ClientId,
    //     BarberShopId,
    //     Date,
    //     StartTime,
    //     TotalDuration,
    //     Notes,
    //     PaymentType,
    //     TotalPrice,
    //     Status,
    //     GetServicesIntoDto()
    // );
}
