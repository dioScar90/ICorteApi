using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IConversationRepository
    : IBasePrimaryKeyRepository<Conversation, int>
{
}
