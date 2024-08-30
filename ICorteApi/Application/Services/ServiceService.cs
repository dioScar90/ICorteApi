using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository repository, IServiceErrors errors)
    : BaseService<Service>(repository), IServiceService
{
    private readonly IServiceErrors _errors = errors;

    new private readonly IServiceRepository _repository = repository;
    
    public async Task<Service?> CreateAsync(ServiceDtoRequest dto, int barberShopId)
    {
        var service = new Service(dto, barberShopId);
        return await CreateAsync(service);
    }
    
    public async Task<Service?> GetByIdAsync(int id, int barberShopId)
    {
        return await GetByIdAsync(x => x.Id == id && x.BarberShopId == barberShopId);
    }
    
    public async Task<Service[]> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
    }
    
    public async Task<bool> UpdateAsync(ServiceDtoRequest dto, int id, int barberShopId)
    {
        var service = await GetByIdAsync(id, barberShopId);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);
        
        service.UpdateEntityByDto(dto);
        return await UpdateAsync(service);
    }
    
    public async Task<bool> DeleteAsync(int id, int barberShopId, bool forceDelete = false)
    {
        var service = await GetByIdAsync(id, barberShopId);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);
            
        var thereAreAppointments = !forceDelete && await _repository.CheckCorrelatedAppointmentsAsync(service.Id);

        if (thereAreAppointments)
        {
            var appointments = await _repository.GetCorrelatedAppointmentsAsync(service.Id);
            var dates = appointments.Select(a => a.Date);

            _errors.ThrowThereAreStillAppointmentsException([..dates]);
        }
        
        return await DeleteAsync(service);
    }
}
