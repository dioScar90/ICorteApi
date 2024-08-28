using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository, IServiceRepository serviceRepository)
    : BaseService<Appointment>(repository), IAppointmentService
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    new private readonly IAppointmentRepository _repository = repository;
    public async Task<ISingleResponse<Appointment>> CreateAsync(AppointmentDtoRequest dtoRequest, int clientId)
    {
        if (dtoRequest is not AppointmentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        if (dto.ServiceIds.Length == 0)
            throw new ArgumentException("Selecione pelo menos um serviço", nameof(dtoRequest));

        var services = await GetSpecificServicesByIdsAsync(dto.ServiceIds);
        var entity = new Appointment(dto, services, clientId);

        return await CreateAsync(entity);
    }

    private async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids) =>
        await _serviceRepository.GetSpecificServicesByIdsAsync(ids);

    public async Task<ISingleResponse<Appointment>> GetByIdAsync(int id)
    {
        var resp = await _repository.GetByIdWithServicesAsync(id);
        
        if (!resp.IsSuccess)
            return resp;
            
        return resp;
    }

    private static void CheckClientIdAsync(Appointment appointment, int clientId)
    {
        if (appointment.ClientId != clientId)
            throw new UnauthorizedException("Agendamento não pertence ao usuário");
    }
    
    public async Task<IResponse> UpdateAsync(AppointmentDtoRequest dtoRequest, int id, int clientId)
    {
        var resp = await _repository.GetByIdWithServicesAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var appointment = resp.Value!;
        CheckClientIdAsync(appointment, clientId);
        
        var currentServiceIds = appointment.Services.Select(s => s.Id).ToArray();
        
        var serviceIdsToAdd = dtoRequest.ServiceIds.Except(currentServiceIds).ToArray();
        var serviceIdsToRemove = currentServiceIds.Except(dtoRequest.ServiceIds).ToArray();

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
            appointment.AddServices(await GetSpecificServicesByIdsAsync(serviceIdsToAdd));

        appointment.UpdateEntityByDto(dtoRequest);
        return await UpdateAsync(appointment);
    }
    
    public async Task<IResponse> DeleteAsync(int id, int clientId)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var appointment = resp.Value!;
        CheckClientIdAsync(appointment, clientId);

        return await DeleteAsync(appointment);
    }
}
