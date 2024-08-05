using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class ServiceAppointmentService(IServiceAppointmentRepository serviceAppointmentRepository)
    : BaseCompositeKeyService<ServiceAppointment, int, int>(serviceAppointmentRepository), IServiceAppointmentService
{
    public override async Task<ISingleResponse<ServiceAppointment>> GetByIdAsync(int appointmentId, int serviceId)
    {
        return await _repository.GetByIdAsync(x => x.AppointmentId == appointmentId && x.ServiceId == serviceId);
    }
    
    public override async Task<IResponse> UpdateAsync(IDtoRequest<ServiceAppointment> dto, int appointmentId, int serviceId)
    {
        var resp = await GetByIdAsync(appointmentId, serviceId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);
        
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
