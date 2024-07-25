using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IConversationParticipantRepository
    : IBaseCompositeKeyRepository<ConversationParticipant, int, int>
{
}
