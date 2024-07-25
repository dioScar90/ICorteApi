using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IMessageRepository
    : IBasePrimaryKeyRepository<Message, int>
{
}
