using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class ConversationParticipantRepository(AppDbContext context)
    : BaseCompositeKeyRepository<ConversationParticipant, int, int>(context), IConversationParticipantRepository
{
}
