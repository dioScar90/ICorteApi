using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class PaymentService(
    IPaymentRepository repository,
    IValidator<PaymentDtoCreate> createValidator,
    IPaymentErrors errors)
    : BaseService<Payment>(repository), IPaymentService
{
    private readonly IValidator<PaymentDtoCreate> _createValidator = createValidator;
    private readonly IPaymentErrors _errors = errors;

    public async Task<PaymentDtoResponse> CreateAsync(PaymentDtoCreate dto, int appointmentId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var payment = new Payment(dto, appointmentId);
        return (await CreateAsync(payment))!.CreateDto();
    }

    public async Task<PaymentDtoResponse> GetByIdAsync(int id, int appointmentId)
    {
        var payment = await GetByIdAsync(id);

        if (payment is null)
            _errors.ThrowNotFoundException();

        if (payment!.AppointmentId != appointmentId)
            _errors.ThrowPaymentNotBelongsToAppointmentException(appointmentId);

        return payment.CreateDto();
    }

    public async Task<PaymentDtoResponse[]> GetAllAsync(int? page, int? pageSize, int appointmentId)
    {
        var payments = await GetAllAsync(new(page, pageSize, x => x.AppointmentId == appointmentId));
        return [..payments.Select(payment => payment.CreateDto())];
    }

    public async Task<bool> DeleteAsync(int id, int appointmentId)
    {
        var payment = await GetByIdAsync(id);

        if (payment is null)
            _errors.ThrowNotFoundException();

        if (payment!.AppointmentId != appointmentId)
            _errors.ThrowPaymentNotBelongsToAppointmentException(appointmentId);

        return await DeleteAsync(payment);
    }
}
