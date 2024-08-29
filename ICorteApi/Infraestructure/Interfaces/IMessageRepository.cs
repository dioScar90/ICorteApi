using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IMessageRepository
    : IBaseRepository<Message>
{
    Task<IResponse> MarkMessageAsReadAsync(int[] messageIds, int senderId);
    Task<MessageDtoResponse[]> GetLastMessagesAsync(int appointmentId, int senderId, int? lastMessageId);
}
