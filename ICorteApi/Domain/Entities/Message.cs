using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class Message : BaseEntity, IPrimaryKeyEntity<int>
{
    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public int SenderId { get; set; }
    public Person Sender { get; set; }
    
    public int Key => Id;
}
