using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class ConversationRepository(AppDbContext context)
    : BasePrimaryKeyRepository<Conversation, int>(context), IConversationRepository
{
}
