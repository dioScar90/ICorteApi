using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public int SenderId { get; set; }
    public Person Sender { get; set; }
}
