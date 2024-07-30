using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class ConversationParticipantService(IConversationParticipantRepository conversationParticipantRepository)
    : BaseCompositeKeyService<ConversationParticipant, int, int>(conversationParticipantRepository), IConversationParticipantService
{
    public override async Task<ISingleResponse<ConversationParticipant>> GetByIdAsync(int conversationId, int participantId)
    {
        return await conversationParticipantRepository.GetByIdAsync(x => x.ConversationId == conversationId && x.ParticipantId == participantId);
    }
    
    public override async Task<IResponse> UpdateAsync(int conversationId, int participantId, IDtoRequest<ConversationParticipant> dto)
    {
        var resp = await GetByIdAsync(conversationId, participantId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);
        
        return await UpdateEntityAsync(entity);
    }

    public override async Task<IResponse> DeleteAsync(int conversationId, int participantId)
    {
        var resp = await GetByIdAsync(conversationId, participantId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        return await DeleteEntityAsync(entity);
    }
}
