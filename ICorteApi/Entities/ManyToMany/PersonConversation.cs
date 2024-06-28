namespace ICorteApi.Entities;

public class PersonConversation : IBaseTableEntity
{
    public int ParticipantId { get; set; }
    public Person Participant { get; set; }

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public bool IsActive { get; set; } = true;
}
