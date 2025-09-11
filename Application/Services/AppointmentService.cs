using FluentValidation;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(
    AppDbContext context,
    IValidator<AppointmentDtoCreate> createValidator,
    IValidator<AppointmentDtoUpdate> updateValidator,
    IServiceService serviceService,
    IAppointmentErrors errors)
    : BaseService<Appointment>(context), IAppointmentService
{
    private readonly IServiceService _serviceService = serviceService;
    private readonly IValidator<AppointmentDtoCreate> _createValidator = createValidator;
    private readonly IValidator<AppointmentDtoUpdate> _updateValidator = updateValidator;
    private readonly IAppointmentErrors _errors = errors;

    private static bool IsServicesFromUniqueBarberShopId(Service[] services)
    {
        var ids = services.Select(s => s.BarberShopId).ToHashSet();
        return ids.Count == 1;
    }

    public async Task<AppointmentDtoResponse> CreateAsync(AppointmentDtoCreate dto, int clientId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);

        if (dto.ServiceIds.Length == 0)
            _errors.ThrowEmptyServicesException();

        var services = await GetSpecificServicesByIdsAsync(dto.ServiceIds);

        if (!IsServicesFromUniqueBarberShopId(services))
            _errors.ThrowNotBarberShopIdsUniqueFromServicesException();

        var appointment = new Appointment(dto, services, clientId);
        return (await CreateAsync(appointment))!.CreateDto();
    }
    
    public async Task<Appointment[]> GetAppointmentsByDateAsync(int barberShopId, DateOnly date)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.BarberShopId == barberShopId && x.Date == date)
            .OrderBy(x => x.Date)
            .ToArrayAsync() ?? [];
    }

    private async Task<Appointment?> GetAppointmentWithServicesAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Services)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<AppointmentDtoResponse> GetByIdWithServicesAsync(int id)
    {
        var appointment = await GetAppointmentWithServicesAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();

        return appointment!.CreateDto();
    }

    private async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        return await _serviceService.GetSpecificServicesByIdsAsync(ids);
    }

    public async Task<AppointmentDtoResponse> GetByIdAsync(int id)
    {
        var appointment = await base.GetByIdAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
        
        return appointment!.CreateDto();
    }

    public async Task<PaginationResponse<AppointmentDtoResponse>> GetAllAsync(int? page, int? pageSize, int clientId)
    {
        var response = await GetAllAsync(new(page, pageSize, x => x.ClientId == clientId, new(x => x.Date)));
        
        return new(
            [..response.Items.Select(service => service.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }

    public async Task<bool> UpdateAsync(AppointmentDtoUpdate dto, int id, int clientId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var appointment = await GetAppointmentWithServicesAsync(id);

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

                appointment!.AddServices(servicesToAdd);
            }

        appointment.UpdateEntityByDto(dto);
        return await UpdateAsync(appointment);
    }

    public async Task<bool> UpdatePaymentTypeAsync(AppointmentPaymentTypeDtoUpdate dto, int id, int clientId)
    {
        var appointment = await _dbSet.FindAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
        
        if (appointment!.ClientId != clientId)
            _errors.ThrowAppointmentNotBelongsToClientException(clientId);

        appointment.UpdateEntityByDto(dto);
        
        appointment.UpdateEntityByDto(dto);
        return await UpdateAsync(appointment);
    }
    
    public async Task<bool> DeleteAsync(int id, int clientId)
    {
        var appointment = await _dbSet.FindAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
        
        if (appointment!.ClientId != clientId)
            _errors.ThrowAppointmentNotBelongsToClientException(clientId);

        return await DeleteAsync(appointment);
    }

    // Task<Infraestructure.Repositories.PaginationResponse<AppointmentDtoResponse>> IAppointmentService.GetAllAsync(int? page, int? pageSize, int clientId)
    // {
    //     throw new NotImplementedException();
    // }
}
