using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IChatService : IService<Message>
{
    Task<ISingleResponse<Message>> CreateAsync(IDtoRequest<Message> dtoRequest, int appointmentId, int senderId);
    Task<ISingleResponse<Message>> GetByIdAsync(int id, int appointmentId);
    Task<ICollectionResponse<Message>> GetAllAsync(int? page, int? pageSize, int appointmentId);
    Task<IResponse> DeleteAsync(int id, int appointmentId);
}
