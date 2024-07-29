using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Conversation : BasePrimaryKeyEntity<int>
{
    public DateTime? LastMessageAt { get; set; }

    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = [];
}
