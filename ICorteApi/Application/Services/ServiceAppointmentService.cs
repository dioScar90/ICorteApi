using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class ServiceAppointmentService(IServiceAppointmentRepository serviceAppointmentRepository)
    : BaseCompositeKeyService<ServiceAppointment, int, int>(serviceAppointmentRepository), IServiceAppointmentService
{
    public override async Task<ISingleResponse<ServiceAppointment>> GetByIdAsync(int appointmentId, int serviceId)
    {
        return await serviceAppointmentRepository.GetByIdAsync(x => x.AppointmentId == appointmentId && x.ServiceId == serviceId);
    }
    
    public override async Task<IResponse> UpdateAsync(int appointmentId, int serviceId, IDtoRequest<ServiceAppointment> dto)
    {
        var resp = await GetByIdAsync(appointmentId, serviceId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);
        
        return await UpdateEntityAsync(entity);
    }

    public override async Task<IResponse> DeleteAsync(int appointmentId, int serviceId)
    {
        var resp = await GetByIdAsync(appointmentId, serviceId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        return await DeleteEntityAsync(entity);
    }
}
