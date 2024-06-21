namespace ICorteApi.Entities;

public class Conversation : BaseEntity
{
    public override int Id { get; set; }
    public int BarberId { get; set; }
    public int ClientId { get; set; }
    
    // Navigation Properties
    public Barber Barber { get; set; }
    public Client Client { get; set; }
    public ICollection<Message> Messages { get; set; } = [];
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