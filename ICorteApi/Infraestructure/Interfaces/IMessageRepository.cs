namespace ICorteApi.Infraestructure.Interfaces;

public interface IMessageRepository
    : IBaseRepository<Message>
{
    Task<bool> MarkMessageAsReadAsync(int[] messageIds, int senderId);
    Task<MessageDtoResponse[]> GetLastMessagesAsync(int appointmentId, int senderId, int? lastMessageId);
    Task<bool> CanSendMessageAsync(int appointmentId, int userId);
}
