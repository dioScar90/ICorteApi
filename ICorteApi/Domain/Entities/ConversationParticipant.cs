using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class ConversationParticipant : BaseHardCrudEntity, ICompositeKeyEntity<int, int>
{
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public int ParticipantId { get; set; }
    public Person Participant { get; set; }
    
    public int Key1 => ConversationId;
    public int Key2 => ParticipantId;
}
