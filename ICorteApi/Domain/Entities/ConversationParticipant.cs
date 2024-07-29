using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class ConversationParticipant : CompositeKeyEntity<int, int>
{
    // public int ConversationId => Id1;
    public int ConversationId { get => Id1; set => Id1 = value; }
    public Conversation Conversation { get; set; }

    // public int ParticipantId => Id2;
    public int ParticipantId { get => Id2; set => Id2 = value; }
    public Person Participant { get; set; }
}
