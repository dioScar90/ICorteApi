using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository)
    : BasePrimaryKeyService<Appointment, int>(repository), IAppointmentService
{
    public async Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dto, int clientId, int barberShopId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.ClientId = clientId;
        entity.BarberShopId = barberShopId;
        
        return await CreateByEntityAsync(entity);
    }
}
