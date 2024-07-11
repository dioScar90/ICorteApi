using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class PersonConversation : BaseHardCrudEntity
{
    public int ParticipantId { get; set; }
    public Person Participant { get; set; }

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }
}
