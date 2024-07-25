using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class ConversationParticipantService(IConversationParticipantRepository conversationParticipantRepository)
    : BaseCompositeKeyService<ConversationParticipant, int, int>(conversationParticipantRepository), IConversationParticipantService
{
}
