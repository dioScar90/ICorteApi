using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Conversation : BaseEntity
{
    public DateTime? LastMessageAt { get; set; }

    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = [];
}
