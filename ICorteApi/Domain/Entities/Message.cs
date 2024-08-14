using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Message : BasePrimaryKeyEntity<Message, int>
{
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public bool IsRead { get; private set; } = false;

    public int AppointmentId { get; init; }
    public Appointment Appointment { get; init; }

    public int SenderId { get; init; }
    public User Sender { get; init; }

    private Message() {}

    public Message(MessageDtoRequest dto, int? appointmentId = null, int? senderId = null)
    {
        Content = dto.Content;
        SentAt = dto.SentAt;

        AppointmentId = appointmentId ?? default;
        SenderId = senderId ?? default;
    }

    private void UpdateByMessageDto(MessageDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Content = dto.Content;
        
        UpdatedAt = utcNow;
    }

    private void UpdateIsReadProp(MessageIsReadDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        IsRead = IsRead || dto.IsRead;
        
        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Message> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is MessageDtoRequest dto)
            UpdateByMessageDto(dto, utcNow);
        
        if (requestDto is MessageIsReadDtoRequest isReadDto)
            UpdateIsReadProp(isReadDto, utcNow);
            
        throw new Exception("Dados enviados inválidos");
    }
}
