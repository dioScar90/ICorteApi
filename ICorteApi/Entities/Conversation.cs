namespace ICorteApi.Entities;

public class Conversation : BaseEntity
{
    public ICollection<Message> Messages { get; set; } = [];
    // public ICollection<User> Participants { get; set; } = [];
    public ICollection<PersonConversation> PersonConversations { get; set; } = [];
}
