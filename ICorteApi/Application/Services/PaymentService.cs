using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class PaymentService(IPaymentRepository repository, IPaymentErrors errors)
    : BaseService<Payment>(repository), IPaymentService
{
    private readonly IPaymentErrors _errors = errors;

    public async Task<Payment?> CreateAsync(PaymentDtoRequest dto, int appointmentId)
    {
        var payment = new Payment(dto, appointmentId);
        return await CreateAsync(payment);
    }

    public async Task<Payment?> GetByIdAsync(int id, int appointmentId)
    {
        var payment = await GetByIdAsync(id);
        
        if (payment is null)
            _errors.ThrowNotFoundException();

        if (payment!.AppointmentId != appointmentId)
            _errors.ThrowPaymentNotBelongsToAppointmentException(appointmentId);

        return payment;
    }
    
    public async Task<Payment[]> GetAllAsync(int? page, int? pageSize, int appointmentId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.AppointmentId == appointmentId));
    }

    public async Task<bool> DeleteAsync(int id, int appointmentId)
    {
        var payment = await GetByIdAsync(id, appointmentId);
        
        if (payment is null)
            _errors.ThrowNotFoundException();

        if (payment!.AppointmentId != appointmentId)
            _errors.ThrowPaymentNotBelongsToAppointmentException(appointmentId);
            
        return await DeleteAsync(payment);
    }
}
