using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(
    IServiceRepository repository,
    IValidator<ServiceDtoCreate> createValidator,
    IValidator<ServiceDtoUpdate> updateValidator,
    IServiceErrors errors)
    : BaseService<Service>(repository), IServiceService
{
    private readonly IValidator<ServiceDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ServiceDtoUpdate> _updateValidator = updateValidator;
    private readonly IServiceErrors _errors = errors;

    new private readonly IServiceRepository _repository = repository;
    
    public async Task<ServiceDtoResponse> CreateAsync(ServiceDtoCreate dto, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var service = new Service(dto, barberShopId);
        return (await CreateAsync(service))!.CreateDto();
    }
    
    public async Task<ServiceDtoResponse> GetByIdAsync(int id, int barberShopId)
    {
        var service = await GetByIdAsync(id);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);

        return service.CreateDto();
    }
    
    public async Task<PaginationResponse<ServiceDtoResponse>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        var response = await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId, new(x => x.Name)));
        
        return new(
            [..response.Data.Select(service => service.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }
    
    public async Task<bool> UpdateAsync(ServiceDtoUpdate dto, int id, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var service = await GetByIdAsync(id);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);
        
        service.UpdateEntityByDto(dto);
        return await UpdateAsync(service);
    }
    
    public async Task<bool> DeleteAsync(int id, int barberShopId, bool forceDelete = false)
    {
        var service = await GetByIdAsync(id);

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
