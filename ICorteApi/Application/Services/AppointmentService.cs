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

    private static bool IsServicesFromUniqueBarberShopId(Service[] services)
    {
        var ids = services.Select(s => s.BarberShopId).ToHashSet();
        return ids.Count == 1;
    }

    public async Task<ISingleResponse<Appointment>> CreateAsync(AppointmentDtoRequest dtoRequest, int clientId)
    {
        if (dtoRequest is not AppointmentDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        if (dto.ServiceIds.Length == 0)
            throw new ArgumentException("Selecione pelo menos um serviço", nameof(dtoRequest));

        var services = await GetSpecificServicesByIdsAsync(dto.ServiceIds);

        if (!IsServicesFromUniqueBarberShopId(services))
            throw new ArgumentException("Serviços escolhidos precisam todos pertencer à mesma barbearia", nameof(dtoRequest));

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
    
    public async Task<IResponse> UpdateAsync(AppointmentDtoRequest dtoRequest, int id, int clientId)
    {
        var resp = await _repository.GetByIdWithServicesAsync(id);

        if (!resp.IsSuccess)
            return resp;

        var appointment = resp.Value!;
        
        if (appointment.ClientId != clientId)
            throw new UnauthorizedException("Agendamento não pertence ao usuário");
        
        var currentServiceIds = appointment.Services.Select(s => s.Id).ToArray();
        
        var serviceIdsToRemove = currentServiceIds.Except(dtoRequest.ServiceIds).ToArray();
        var serviceIdsToAdd = dtoRequest.ServiceIds.Except(currentServiceIds).ToArray();

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
        {
            var servicesToAdd = await GetSpecificServicesByIdsAsync(serviceIdsToAdd);

            if (!IsServicesFromUniqueBarberShopId(servicesToAdd))
                throw new ArgumentException("Serviços escolhidos precisam todos pertencer à mesma barbearia", nameof(dtoRequest));

            appointment.AddServices(servicesToAdd);
        }
        
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
            throw new UnauthorizedException("Agendamento não pertence ao usuário");

        return await DeleteAsync(appointment);
    }
}
