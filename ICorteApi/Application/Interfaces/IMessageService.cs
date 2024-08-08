using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IMessageService
    : IBasePrimaryKeyService<Message, int>, IHasTwoForeignKeyService<Message, int, int>
{
    new Task<ISingleResponse<Message>> CreateAsync(IDtoRequest<Message> dto, int appointmentId, int senderId);
}
