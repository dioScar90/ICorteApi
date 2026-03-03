using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(
    AppDbContext context,
    ILogger<AppointmentService> _logger,
    UserService _userService,
    ServiceService _serviceService)
    : BaseService<Appointment>(context)
{
    public async Task<AppointmentDtoResponse?> CreateAsync(AppointmentDtoRequest dto)
    {
        var services = await _serviceService.GetSpecificServicesByIdsAsync([.. dto.Services.Select(s => s.Id)]);
        
        dto = dto with { ClientId = await _userService.GetMyUserIdAsync() };
        var appointment = new Appointment(dto, services);
        
        _dbSet.Add(appointment);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Appointment persisted in database with Id={Id}", appointment.Id);
        return await GetByIdAsync(appointment.Id);
    }
    
    public record Includes(bool Collections = false);
    
    public async Task<bool> AppointmentBelongsToClientAsync(int appointmentId, int? clientId = null)
    {
        _logger.LogDebug("Checking if Appointment with Id={Id} belongs to Client with Id={ClientId}", appointmentId, clientId);
        
        clientId ??= await _userService.GetMyUserIdAsync();
        
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == appointmentId && x.ClientId == clientId);
    }

    private async Task<Appointment?> FindEntityAsync(int id, Includes? includes = null)
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
        
        return await query
            .FirstOrDefaultAsync();
    }

    public async Task<AppointmentDtoResponse?> GetByIdAsync(int id, Includes? includes = null)
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

        return await query
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
        var servicesToAdd = await _serviceService.GetSpecificServicesByIdsAsync(serviceIdsToAdd);

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
            appointment.AddServices(servicesToAdd);
    }

    public async Task<bool> UpdateAsync(AppointmentDtoRequest dto, int id)
    {
        var appointment = await FindEntityAsync(id);

        if (appointment is null)
            return false;
        
        dto = dto with { ClientId = appointment.ClientId };
        
        await UpdateAppointmentServicesAsync(appointment, dto);
        
        _logger.LogDebug("Updating Appointment with Id={Id}", id);

        appointment.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> UpdatePaymentTypeAsync(AppointmentPaymentTypeDtoUpdateRequest dto, int id)
    {
        var appointment = await FindEntityAsync(id);

        if (appointment is null)
            return false;
            
        dto = dto with { ClientId = appointment.ClientId };

        appointment.UpdateEntity(dto);
        return await SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await FindEntityAsync(id);

        if (appointment is null)
            return false;
        
        _logger.LogDebug("Deleting Appointment with Id={Id}", id);

        _dbSet.Remove(appointment);
        return await SaveChangesAsync();
    }
}
