using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ConversationService(IConversationRepository repository)
    : BasePrimaryKeyService<Conversation, int>(repository), IConversationService
{
}
