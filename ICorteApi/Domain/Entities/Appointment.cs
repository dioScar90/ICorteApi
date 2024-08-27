using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Appointment : BaseEntity<Appointment>
{
    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public string? Notes { get; private set; }
    public decimal TotalPrice { get; private set; }
    public AppointmentStatus? Status { get; private set; }

    public int ClientId { get; init; }
    public User Client { get; set; }

    public ICollection<Message> Messages { get; init; } = [];
    public ICollection<Payment> Payments { get; init; } = [];
    public ICollection<ServiceAppointment> ServiceAppointments { get; init; } = [];

    private Appointment() { }

    public Appointment(AppointmentDtoRequest dto, int? clientId = null)
    {
        Date = dto.Date;
        StartTime = dto.StartTime;
        EndTime = StartTime.AddHours(1);
        Notes = dto.Notes;
        TotalPrice = 99.9M;
        // Status = dto.Status;

        ServiceAppointments = dto.ServiceIds.ToHashSet().Select(sId => new ServiceAppointment(sId, this)).ToArray();

        Status = AppointmentStatus.Pending;

        ClientId = clientId ?? default;
    }

    private (ServiceAppointment[], int[]) GetServiceAppointmentsToAddAndToRemove(HashSet<int> serviceIds)
    {
        List<ServiceAppointment> itemsToRemove = [];
        List<int> itemsToKeep = [];

        foreach (var item in ServiceAppointments)
        {
            if (serviceIds.Contains(item.ServiceId))
                itemsToKeep.Add(item.ServiceId);
            else
                itemsToRemove.Add(item);
        }

        return ([..itemsToRemove], [..serviceIds.Where(sId => !itemsToKeep.Contains(sId))]);
    }

    private void UpdateByAppointmentDto(AppointmentDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Date = dto.Date;
        StartTime = dto.StartTime;
        // EndTime = dto.EndTime;
        Notes = dto.Notes;
        // TotalPrice = dto.TotalPrice;
        // Status = dto.Status;

        // ServiceAppointments.Where(sa => dto.ServiceIds.Contains(sa.ServiceId));

        // ServiceAppointments.Remove()


        var (ServiceAppointmentsToRemove, serviceIdsToAdd) = GetServiceAppointmentsToAddAndToRemove([..dto.ServiceIds]);

        foreach (var toRemove in ServiceAppointmentsToRemove)
        {
            // Console.WriteLine("\n\n\n");
            // Console.WriteLine(toRemove.ServiceId);

            ServiceAppointments.Remove(toRemove);
        }
        
        foreach (var toAdd in serviceIdsToAdd)
            ServiceAppointments.Add(new(toAdd, this));

        Console.WriteLine("\n\n\n");
        Console.WriteLine(string.Join(",", ServiceAppointments.Select(sa => sa.ServiceId)));
        
        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Appointment> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case AppointmentDtoRequest dto:
                UpdateByAppointmentDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override AppointmentDtoResponse CreateDto() =>
        new(
            Id,
            Date,
            StartTime,
            ServiceAppointments.Aggregate(StartTime, (acc, curr) => acc.Add(curr.Service.Duration)),
            Notes,
            ServiceAppointments.Aggregate(0M, (acc, curr) => acc + curr.Service.Price),
            Status ?? AppointmentStatus.Pending
        );
}
