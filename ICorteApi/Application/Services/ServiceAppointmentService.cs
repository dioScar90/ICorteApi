using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceAppointmentService(IServiceAppointmentRepository repository)
    : BaseCompositeKeyService<ServiceAppointment, int, int>(repository), IServiceAppointmentService
{
    public override async Task<ISingleResponse<ServiceAppointment>> GetByIdAsync(int appointmentId, int serviceId)
    {
        return await _repository.GetByIdAsync(x => x.AppointmentId == appointmentId && x.ServiceId == serviceId);
    }

    public override async Task<IResponse> UpdateAsync(IDtoRequest<ServiceAppointment> dtoRequest, int appointmentId, int serviceId)
    {
        var resp = await GetByIdAsync(appointmentId, serviceId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await _repository.UpdateAsync(entity);
    }

    public override async Task<IResponse> DeleteAsync(int appointmentId, int serviceId)
    {
        var resp = await GetByIdAsync(appointmentId, serviceId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await _repository.DeleteAsync(entity);
    }
}
