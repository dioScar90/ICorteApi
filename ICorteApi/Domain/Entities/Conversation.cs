using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

// Conversa
public class Conversation : BaseEntity
{
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<PersonConversation> PersonConversations { get; set; } = [];
}
