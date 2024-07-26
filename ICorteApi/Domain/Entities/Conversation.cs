using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class Conversation : BasePrimaryKeyEntity<int>
{
    public DateTime? LastMessageAt { get; set; }

    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = [];
    
    // public int Key => Id;
}
