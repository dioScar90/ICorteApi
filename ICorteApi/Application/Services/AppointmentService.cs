using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository)
    : BasePrimaryKeyService<Appointment, int>(repository), IAppointmentService
{
    public async Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dtoRequest, int clientId)
    {
        if (dtoRequest is not AppointmentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Appointment(dto, clientId);
        return await CreateByEntityAsync(entity);
    }
}
