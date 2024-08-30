using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(IAppointmentRepository repository, IServiceRepository serviceRepository, IAppointmentErrors errors)
    : BaseService<Appointment>(repository), IAppointmentService
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    new private readonly IAppointmentRepository _repository = repository;
    private readonly IAppointmentErrors _errors = errors;

    private static bool IsServicesFromUniqueBarberShopId(Service[] services)
    {
        var ids = services.Select(s => s.BarberShopId).ToHashSet();
        return ids.Count == 1;
    }

    public async Task<Appointment?> CreateAsync(AppointmentDtoRequest dto, int clientId)
    {
        if (dto.ServiceIds.Length == 0)
            _errors.ThrowEmptyServicesException();

        var services = await GetSpecificServicesByIdsAsync(dto.ServiceIds);

        if (!IsServicesFromUniqueBarberShopId(services))
            _errors.ThrowNotBarberShopIdsUniqueFromServicesException();

        var appointment = new Appointment(dto, services, clientId);
        return await CreateAsync(appointment);
    }

    private async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        return await _serviceRepository.GetSpecificServicesByIdsAsync(ids);
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdWithServicesAsync(id);
    }

    public async Task<bool> UpdateAsync(AppointmentDtoRequest dto, int id, int clientId)
    {
        var appointment = await _repository.GetByIdWithServicesAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
        
        if (appointment!.ClientId != clientId)
            _errors.ThrowAppointmentNotBelongsToClientException(clientId);

        var currentServiceIds = appointment.Services.Select(s => s.Id).ToArray();

        var serviceIdsToRemove = currentServiceIds.Except(dto.ServiceIds).ToArray();
        var serviceIdsToAdd = dto.ServiceIds.Except(currentServiceIds).ToArray();

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
        {
            var servicesToAdd = await GetSpecificServicesByIdsAsync(serviceIdsToAdd);

            if (!IsServicesFromUniqueBarberShopId(servicesToAdd))
                _errors.ThrowNotBarberShopIdsUniqueFromServicesException();

            appointment.AddServices(servicesToAdd);
        }

        appointment.UpdateEntityByDto(dto);
        return await UpdateAsync(appointment);
    }

    public async Task<bool> DeleteAsync(int id, int clientId)
    {
        var appointment = await GetByIdAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
        
        if (appointment!.ClientId != clientId)
            _errors.ThrowAppointmentNotBelongsToClientException(clientId);

        return await DeleteAsync(appointment);
    }
}
