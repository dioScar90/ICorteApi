using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class MessageService(IMessageRepository repository)
    : BasePrimaryKeyService<Message, int>(repository), IMessageService
{
}
