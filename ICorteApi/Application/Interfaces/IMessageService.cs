namespace ICorteApi.Application.Interfaces;

public interface IMessageService : IService<Message>
{
    Task<MessageDtoResponse> CreateAsync(MessageDtoCreate dtoRequest, int appointmentId, int senderId);
    Task<MessageDtoResponse> GetByIdAsync(int id, int appointmentId);
    Task<PaginationResponse<MessageDtoResponse>> GetAllAsync(int? page, int? pageSize, int appointmentId);
    Task<bool> DeleteAsync(int id, int appointmentId);
}
