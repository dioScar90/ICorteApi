using FluentValidation;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(
    AppDbContext context,
    IValidator<ServiceDtoCreate> createValidator,
    IValidator<ServiceDtoUpdate> updateValidator,
    IServiceErrors errors)
    : BaseService<Service>(context), IServiceService
{
    private readonly IValidator<ServiceDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ServiceDtoUpdate> _updateValidator = updateValidator;
    private readonly IServiceErrors _errors = errors;
    
    public async Task<ServiceDtoResponse> CreateAsync(ServiceDtoCreate dto, int barberShopId)
    {
        dto.ThrowExceptionIfInvalid(_createValidator, _errors);
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
            [..response.Items.Select(service => service.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }
    
    public async Task<bool> UpdateAsync(ServiceDtoUpdate dto, int id, int barberShopId)
    {
        dto.ThrowExceptionIfInvalid(_updateValidator, _errors);

        var service = await GetByIdAsync(id);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);
        
        service.UpdateEntityByDto(dto);
        return await UpdateAsync(service);
    }
    public async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        var hashIds = ids.ToHashSet();
        return await _dbSet.Where(x => hashIds.Contains(x.Id)).ToArrayAsync();
    }

    private async Task<bool> CheckCorrelatedAppointmentsAsync(int id) =>
        await _dbSet.AnyAsync(s => s.Id == id && s.Appointments.Any());

    private async Task<Appointment[]> GetCorrelatedAppointmentsAsync(int id) =>
        await _dbSet
            .Where(s => s.Id == id)
            .SelectMany(s => s.Appointments)
            .ToArrayAsync();
            
    public async Task<bool> DeleteAsync(int id, int barberShopId, bool forceDelete = false)
    {
        var service = await GetByIdAsync(id);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);

        var thereAreAppointments = !forceDelete && await CheckCorrelatedAppointmentsAsync(service.Id);

        if (thereAreAppointments)
        {
            var appointments = await GetCorrelatedAppointmentsAsync(service.Id);
            var dates = appointments.Select(a => a.Date);

            _errors.ThrowThereAreStillAppointmentsException([.. dates]);
        }

        return await DeleteAsync(service);
    }
}
