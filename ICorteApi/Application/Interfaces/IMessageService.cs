using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IMessageService : IService<Message>
{
    Task<Message?> CreateAsync(MessageDtoRequest dtoRequest, int appointmentId, int senderId);
    Task<Message?> GetByIdAsync(int id, int appointmentId);
    Task<Message[]> GetAllAsync(int? page, int? pageSize, int appointmentId);
    Task<bool> DeleteAsync(int id, int appointmentId);
}
