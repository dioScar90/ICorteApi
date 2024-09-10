using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IPaymentService : IService<Payment>
{
    Task<PaymentDtoResponse> CreateAsync(PaymentDtoCreate dto, int appointmentId);
    Task<PaymentDtoResponse> GetByIdAsync(int id, int appointmentId);
    Task<PaymentDtoResponse[]> GetAllAsync(int? page, int? pageSize, int appointmentId);
    Task<bool> DeleteAsync(int id, int appointmentId);
}
