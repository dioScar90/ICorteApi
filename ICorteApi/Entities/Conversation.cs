namespace ICorteApi.Entities;

public class Conversation : BaseEntity
{
    
    // Navigation Properties
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<User> Participants { get; set; } = [];
}

/*
CHAT GPT:

Conversation: Representa uma conversa entre dois usuários.

Id (int)
Participant1Id (int) - Foreign Key (User)
Participant2Id (int) - Foreign Key (User)
CreatedAt (DateTime)
UpdatedAt (DateTime)
Navigation Properties: List<Message>, Participant1, Participant2
*/