using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class MessageService(IMessageRepository repository)
    : BasePrimaryKeyService<Message, int>(repository), IMessageService
{
    public async Task<ISingleResponse<Message>> CreateAsync(IDtoRequest<Message> dto, int appointmentId, int senderId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.AppointmentId = appointmentId;
        entity.SenderId = senderId;
        
        return await CreateByEntityAsync(entity);
    }
}
