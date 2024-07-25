using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class MessageRepository(AppDbContext context)
    : BasePrimaryKeyRepository<Message, int>(context), IMessageRepository
{
}
