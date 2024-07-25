using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class ConversationService(IConversationRepository conversationRepository)
    : BasePrimaryKeyService<Conversation, int>(conversationRepository), IConversationService
{
}
