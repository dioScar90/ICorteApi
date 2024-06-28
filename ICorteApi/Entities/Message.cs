namespace ICorteApi.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public int SenderId { get; set; }
    public Person Sender { get; set; }
}
