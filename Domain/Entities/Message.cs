using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Entities;

public sealed class Message : BaseEntity<Message, MessageDto>
{
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public bool IsRead { get; private set; } = false;

    public int AppointmentId { get; init; }
    public Appointment Appointment { get; init; }

    public int SenderId { get; init; }
    public User Sender { get; init; }

    private Message() { }

    public Message(MessageDto dto, int? appointmentId = null, int? senderId = null)
    {
        dto.ThrowExceptionIfInvalid(new MessageDtoValidator(), new MessageErrors());

        Content = dto.Content;
        SentAt = dto.SentAt;

        AppointmentId = appointmentId ?? default;
        SenderId = senderId ?? default;
    }
    
    public override void UpdateEntityByDto(MessageDto dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        Content = dto.Content;
        IsRead |= dto.IsRead;

        UpdatedAt = utcNow;
    }

    public override MessageDto CreateDto() => new(
        Id,
        AppointmentId,
        SenderId,
        Content,
        SentAt,
        IsRead,
        Sender.Profile.FirstName,
        Sender.Profile.LastName
    );
}
