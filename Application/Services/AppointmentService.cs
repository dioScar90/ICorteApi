using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(
    AppDbContext context,
    ServiceService serviceService,
    UserService userService,
    ILogger<AppointmentService> logger)
    : BaseService<Appointment>(context)
{
    public async Task<AppointmentDtoResponse?> CreateAsync(
        AppointmentDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var clientId = await userService.GetMyUserIdAsync();

        if (clientId is null)
            return null;
            
        var services = await serviceService.GetSpecificServicesByIdsAsync([.. dto.Services.Select(s => s.Id)], cancellationToken);
        dto = dto with { ClientId = clientId.Value };

        var appointment = new Appointment(dto, services);
        
        dbSet.Add(appointment);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;

        logger.LogInformation("Appointment persisted in database with Id={Id}", appointment.Id);
        return appointment.CreateDto();
    }
    
    public async Task<EntityInfos> GetInfosAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUserId = await userService.GetMyUserIdAsync();
        
        var infos = await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Select(a => new EntityInfos(
                !a.IsDeleted,
                currentUserId != null && a.ClientId == currentUserId
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return infos ?? new();
    }
    
    public async Task<AppointmentDtoResponse?> GetByIdAsync(
        int id, bool withServices = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogDebug("Fetching Appointment with Id={Id} from database", id);
        
        var query = dbSet
            .AsNoTracking()
            .Where(a => a.Id == id);
            
        if (withServices)
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
                a.Status,
                a.Services
                    .Where(_ => withServices)
                    .Select(s => new ServiceDtoResponse(
                        s.Id,
                        s.BarberShopId,
                        s.BarberShop.Name,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    ))
                    .ToArray()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PaginationResponse<AppointmentDtoResponse>> GetAllAsync(
        int? page, int? pageSize,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var clientId = await userService.GetMyUserIdAsync()!;

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
                    a.Status,
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
                        .ToArray()
                ),
                a => a.Services
            ),
            cancellationToken
        );
    }
    
    private async Task UpdateAppointmentServicesAsync(
        Appointment appointment, AppointmentDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentServiceIds = appointment.Services.Select(s => s.Id).ToArray();
        int[] serviceIds = [.. dto.Services.Select(s => s.Id)];
        
        var serviceIdsToRemove = currentServiceIds.Except(serviceIds).ToArray();
        
        var serviceIdsToAdd = serviceIds.Except(currentServiceIds).ToArray();
        var servicesToAdd = await serviceService.GetSpecificServicesByIdsAsync(serviceIdsToAdd, cancellationToken);

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
            appointment.AddServices(servicesToAdd);
    }

    public async Task<bool> UpdateAsync(
        AppointmentDtoRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var appointment = await dbSet.FindAsync([id], cancellationToken);

        if (appointment is null)
            return false;
            
        await UpdateAppointmentServicesAsync(appointment, dto, cancellationToken);
        
        logger.LogDebug("Updating Appointment with Id={Id}", id);

        appointment.UpdateEntity(dto);
        return await SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdatePaymentTypeAsync(
        AppointmentPaymentTypeDtoUpdateRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var appointment = await dbSet.FindAsync([id], cancellationToken);

        if (appointment is null)
            return false;
            
        logger.LogDebug("Updating PaymentType of Appointment with Id={Id}", id);
        
        appointment.UpdateEntity(dto);
        
        return await SaveChangesAsync(cancellationToken);
    }
    
    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var appointment = await dbSet.FindAsync([id], cancellationToken);

        if (appointment is null)
            return false;
        
        logger.LogDebug("Deleting Appointment with Id={Id}", id);

        dbSet.Remove(appointment);
        return await SaveChangesAsync(cancellationToken);
    }
}
