using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Appointment : BasePrimaryKeyEntity<Appointment, int>
{
    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public string? Notes { get; private set; }
    public decimal TotalPrice { get; private set; }
    public AppointmentStatus Status { get; private set; }

    public int ClientId { get; init; }
    public User Client { get; set; }
    
    public ICollection<Message> Messages { get; init; } = [];
    public ICollection<Payment> Payments { get; init; } = [];
    public ICollection<ServiceAppointment> ServiceAppointments { get; init; } = [];

    private Appointment() {}

    public Appointment(AppointmentDtoRequest dto, int? clientId = null)
    {
        Date = dto.Date;
        StartTime = dto.StartTime;
        EndTime = dto.EndTime;
        Notes = dto.Notes;
        TotalPrice = dto.TotalPrice;
        Status = dto.Status;

        ClientId = clientId ?? default;
    }
    
    private void UpdateByAppointmentDto(AppointmentDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Date = dto.Date;
        StartTime = dto.StartTime;
        EndTime = dto.EndTime;
        Notes = dto.Notes;
        TotalPrice = dto.TotalPrice;
        Status = dto.Status;
        
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
            EndTime,
            Notes,
            TotalPrice,
            Status
        );
}
