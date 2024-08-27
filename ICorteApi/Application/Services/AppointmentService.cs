using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository)
    : BaseService<Appointment>(repository), IAppointmentService
{
    new private readonly IAppointmentRepository _repository = repository;
    public async Task<ISingleResponse<Appointment>> CreateAsync(IDtoRequest<Appointment> dtoRequest, int clientId)
    {
        if (dtoRequest is not AppointmentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Appointment(dto, clientId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<Appointment>> GetByIdAsync(int id)
    {
        var resp = await _repository.GetByIdWithServicesAsync(id);
        
        if (!resp.IsSuccess)
            return resp;
            
        return resp;
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<Appointment> dtoRequest, int id, int clientId)
    {
        var resp = await _repository.GetByIdWithServicesAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var appointment = resp.Value!;

        if (appointment.ClientId != clientId)
            return Response.Failure(Error.TEntityNotFound);

        appointment.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(appointment);
    }

    public async Task<IResponse> DeleteAsync(int id, int clientId)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var appointment = resp.Value!;

        if (appointment.ClientId != clientId)
            return Response.Failure(Error.TEntityNotFound);

        return await DeleteAsync(appointment);
    }
}
