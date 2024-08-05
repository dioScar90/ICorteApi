using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ConversationRepository(AppDbContext context)
    : BasePrimaryKeyRepository<Conversation, int>(context), IConversationRepository
{
}
