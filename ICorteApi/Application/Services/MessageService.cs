using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class MessageService(IMessageRepository repository)
    : BasePrimaryKeyService<Message, int>(repository), IMessageService
{
    public async Task<ISingleResponse<Message>> CreateAsync(IDtoRequest<Message> dtoRequest, int appointmentId, int senderId)
    {
        if (dtoRequest is not MessageDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Message(dto, appointmentId, senderId);
        return await CreateByEntityAsync(entity);
    }
}
