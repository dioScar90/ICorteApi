using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class ConversationParticipant : CompositeKeyEntity<int, int>
{
    public int ConversationId { get => Id1; set => Id1 = value; }
    public Conversation Conversation { get; set; }

    public int ParticipantId { get => Id2; set => Id2 = value; }
    public User Participant { get; set; }
}
