using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IMessageService
    : IBasePrimaryKeyService<Message, int>
{
}
