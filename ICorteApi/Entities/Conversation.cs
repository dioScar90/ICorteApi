namespace ICorteApi.Entities;

public class Conversation : BaseEntity
{
    
    // Navigation Properties
    public IEnumerable<Message> Messages { get; set; } = [];
    // public IEnumerable<User> Participants { get; set; } = [];
    public IEnumerable<PersonConversation> PersonConversations { get; set; } = [];
}
