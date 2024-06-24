namespace ICorteApi.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public int SenderId { get; set; }
    public int ConversationId { get; set; }

    // Navigation Properties
    public Conversation Conversation { get; set; }
    public User Sender { get; set; }
}

/*
CHAT GPT:

Possível Entidade Adicional para Mensageria
Para um sistema de mensageria utilizando SignalR, você pode precisar de uma entidade para representar as mensagens e as conversas entre os usuários (clientes e barbeiros).

Message: Representa as mensagens trocadas entre usuários.

Id (int)
SenderId (int) - Foreign Key (User)
ReceiverId (int) - Foreign Key (User)
Content (string)
Timestamp (DateTime)
IsRead (bool)
Navigation Properties: Sender, Receiver
*/