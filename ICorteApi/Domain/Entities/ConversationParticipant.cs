using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class ConversationParticipant : BaseHardCrudEntity
{
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public int ParticipantId { get; set; }
    public Person Participant { get; set; }
}
