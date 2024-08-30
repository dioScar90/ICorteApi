using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IPaymentService : IService<Payment>
{
    Task<Payment?> CreateAsync(PaymentDtoRequest dto, int appointmentId);
    Task<Payment?> GetByIdAsync(int id, int appointmentId);
    Task<Payment[]> GetAllAsync(int? page, int? pageSize, int appointmentId);
    Task<bool> DeleteAsync(int id, int appointmentId);
}
