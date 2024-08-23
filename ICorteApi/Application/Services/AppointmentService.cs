using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository)
    : BaseService<Appointment>(repository), IAppointmentService
{
    public async Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dtoRequest, int clientId)
    {
        if (dtoRequest is not AppointmentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Appointment(dto, clientId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<Appointment>> GetByIdAsync(int id, int clientId)
    {
        return await GetByIdAsync(x => x.Id == id && x.ClientId == clientId);
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<Appointment> dtoRequest, int id, int clientId)
    {
        var resp = await GetByIdAsync(id, clientId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(int id, int clientId)
    {
        var resp = await GetByIdAsync(id, clientId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
