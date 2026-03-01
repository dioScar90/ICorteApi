using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(
    AppDbContext context,
    ILogger<AppointmentService> _logger,
    UserService _userService,
    ServiceService _serviceService,
    AppointmentErrors _errors)
    : BaseService<Appointment>(context)
{
    private static bool IsServicesFromUniqueBarberShopId(Service[] services)
    {
        var ids = services.Select(s => s.BarberShopId).ToHashSet();
        return ids.Count == 1;
    }
    
    public async Task<AppointmentDtoResponse> CreateAsync(AppointmentDtoRequest dto)
    {
        _logger.LogDebug("Starting validation for Appointment {@Appointment}", dto);

        if (dto.Services.Length == 0)
            _errors.ThrowEmptyServicesException();

        var services = await GetSpecificServicesByIdsAsync([.. dto.Services.Select(s => s.Id)]);

        if (!IsServicesFromUniqueBarberShopId(services))
            _errors.ThrowNotBarberShopIdsUniqueFromServicesException();
            
        dto = dto with { ClientId = await _userService.GetMyUserIdAsync() };
        var appointment = new Appointment(dto, services);

        _dbSet.Add(appointment);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Appointment persisted in database with Id={Id}", appointment.Id);
        return await GetByIdAsync(appointment.Id);
    }
    
    private async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        return await _serviceService.GetSpecificServicesByIdsAsync(ids);
    }

    public record Includes(bool Collections = false);

    private async Task<Appointment> FindEntityAsync(int id, Includes? includes = null)
    {
        _logger.LogDebug("Fetching Appointment with Id={Id} from database", id);
        
        includes ??= new();

        var query = _dbSet
            .Where(a => a.Id == id);
            
        if (includes.Collections)
        {
            query = query
                .AsSplitQuery()
                .Include(a => a.Services);
        }

        var appointment = await query
            .FirstOrDefaultAsync();

        if (appointment is null)
            _errors.ThrowNotFoundException();
            
        var clientId = await _userService.GetMyUserIdAsync();

        if (appointment!.ClientId != clientId)
            _errors.ThrowAppointmentNotBelongsToClientException(clientId);

        return appointment;
    }

    public async Task<AppointmentDtoResponse> GetByIdAsync(int id, Includes? includes = null)
    {
        _logger.LogDebug("Fetching Appointment with Id={Id} from database", id);
        
        includes ??= new();

        var query = _dbSet
            .AsNoTracking()
            .Where(a => a.Id == id);

        if (includes.Collections)
        {
            query = query
                .AsSplitQuery()
                .Include(a => a.Services);
        }

        var appointment = await query
            .Select(a => new AppointmentDtoResponse(
                a.Id,
                a.ClientId,
                a.BarberShopId,
                a.Date,
                a.StartTime,
                a.TotalDuration,
                a.Notes,
                a.PaymentType,
                a.TotalPrice,
                a.Services
                    .Where(_ => includes.Collections)
                    .Select(s => new ServiceDtoResponse(
                        s.Id,
                        s.BarberShopId,
                        s.BarberShop.Name,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    ))
                    .ToArray(),
                a.Status
            ))
            .FirstOrDefaultAsync();

        if (appointment is null)
            _errors.ThrowNotFoundException();

        return appointment!;
    }

    public async Task<PaginationResponse<AppointmentDtoResponse>> GetAllAsync(
        int? page, int? pageSize)
    {
        var clientId = await _userService.GetMyUserIdAsync()!;

        return await GetAllAsync<AppointmentDtoResponse>(
            new(
                page,
                pageSize,
                x => x.ClientId == clientId,
                new(x => x.Date),
                a => new(
                    a.Id,
                    a.ClientId,
                    a.BarberShopId,
                    a.Date,
                    a.StartTime,
                    a.TotalDuration,
                    a.Notes,
                    a.PaymentType,
                    a.TotalPrice,
                    a.Services
                        .Select(s => new ServiceDtoResponse(
                            s.Id,
                            s.BarberShopId,
                            s.BarberShop.Name,
                            s.Name,
                            s.Description,
                            s.Price,
                            s.Duration
                        ))
                        .ToArray(),
                    a.Status
                ),
                a => a.Services
            )
        );
    }

    private async Task UpdateAppointmentServicesAsync(Appointment appointment, AppointmentDtoRequest dto)
    {
        var currentServiceIds = appointment.Services.Select(s => s.Id).ToArray();
        int[] serviceIds = [.. dto.Services.Select(s => s.Id)];

        var serviceIdsToRemove = currentServiceIds.Except(serviceIds).ToArray();

        var serviceIdsToAdd = serviceIds.Except(currentServiceIds).ToArray();
        var servicesToAdd = await GetSpecificServicesByIdsAsync(serviceIdsToAdd);

        if (!IsServicesFromUniqueBarberShopId(servicesToAdd))
            _errors.ThrowNotBarberShopIdsUniqueFromServicesException();

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
            appointment.AddServices(servicesToAdd);
    }

    public async Task<bool> UpdateAsync(AppointmentDtoRequest dto, int id)
    {
        _logger.LogDebug("Starting validation for Appointment {@Appointment}", dto);

        var appointment = await FindEntityAsync(id);
        
        dto = dto with { ClientId = appointment.ClientId };

        await UpdateAppointmentServicesAsync(appointment, dto);
        
        _logger.LogDebug("Updating Appointment with Id={Id}", id);
        appointment.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task UpdatePaymentTypeAsync(AppointmentPaymentTypeDtoUpdateRequest dto, int id)
    {
        var appointment = await FindEntityAsync(id);
            
        dto = dto with { ClientId = appointment.ClientId };

        appointment.UpdateEntity(dto);
        await SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var appointment = await FindEntityAsync(id);
        
        _logger.LogDebug("Deleting Appointment with Id={Id}", id);
        await DeleteAsync(appointment);
    }
}
