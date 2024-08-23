using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class PaymentService(IPaymentRepository repository)
    : BaseService<Payment>(repository), IPaymentService
{
    public async Task<ISingleResponse<Payment>> CreateAsync(IDtoRequest<Payment> dtoRequest, int appointmentId)
    {
        if (dtoRequest is not PaymentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Payment(dto, appointmentId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<Payment>> GetByIdAsync(int id, int appointmentId)
    {
        return await GetByIdAsync(x => x.Id == id && x.AppointmentId == appointmentId);
    }
    
    public async Task<ICollectionResponse<Payment>> GetAllAsync(int? page, int? pageSize, int appointmentId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.AppointmentId == appointmentId));
    }

    public async Task<IResponse> DeleteAsync(int id, int appointmentId)
    {
        var resp = await GetByIdAsync(id, appointmentId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
