using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IPaymentService : IService<Payment>
{
    Task<ISingleResponse<Payment>> CreateAsync(IDtoRequest<Payment> dtoRequest, int appointmentId);
    Task<ISingleResponse<Payment>> GetByIdAsync(int id, int appointmentId);
    Task<ICollectionResponse<Payment>> GetAllAsync(int? page, int? pageSize, int appointmentId);
    Task<IResponse> DeleteAsync(int id, int appointmentId);
}
