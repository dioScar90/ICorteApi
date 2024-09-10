using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Message : BaseEntity<Message>
{
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public bool IsRead { get; private set; } = false;

    public int AppointmentId { get; init; }
    public Appointment Appointment { get; init; }

    public int SenderId { get; init; }
    public User Sender { get; init; }

    private Message() { }

    public Message(MessageDtoCreate dto, int? appointmentId = null, int? senderId = null)
    {
        Content = dto.Content;
        SentAt = dto.SentAt;

        AppointmentId = appointmentId ?? default;
        SenderId = senderId ?? default;
    }

    private void UpdateByMessageDto(MessageDtoCreate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Content = dto.Content;

        UpdatedAt = utcNow;
    }

    private void UpdateIsReadProp(MessageDtoIsReadUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        IsRead = IsRead || dto.IsRead;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Message> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case MessageDtoCreate dto:
                UpdateByMessageDto(dto, utcNow);
                break;
            case MessageDtoIsReadUpdate isReadDto:
                UpdateIsReadProp(isReadDto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override MessageDtoResponse CreateDto() =>
        new(
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
