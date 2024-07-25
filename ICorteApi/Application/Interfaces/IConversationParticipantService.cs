using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IConversationParticipantService
    : IBaseCompositeKeyService<ConversationParticipant, int, int>
{
}
