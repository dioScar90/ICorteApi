using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(
    AppDbContext context,
    UserService userService)
    : BaseService<Service>(context)
{
    public async Task<ServiceDtoResponse?> CreateAsync(
        ServiceDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var service = new Service(dto, dto.BarberShopId);
        
        dbSet.Add(service);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return service.CreateDto();
    }
    
    public async Task<EntityInfos> GetEntityInfosAsync(
        int id, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUserId = await userService.GetMyUserIdAsync();
        
        var infos = await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Select(s => new EntityInfos(
                !s.IsDeleted,
                currentUserId != null && s.BarberShopId == currentUserId,
                s.BarberShopId == barberShopId
            ))
            .FirstOrDefaultAsync(cancellationToken);
            
        return infos ?? new();
    }
    
    public async Task<bool> CheckCorrelatedAppointmentsAsync(int id, CancellationToken cancellationToken = default) =>
        await dbSet
            .AsNoTracking()
            .AnyAsync(s => s.Id == id && s.Appointments.Any(), cancellationToken);
            
    public async Task<DateOnly[]> GetDatesFromCorrelatedAppointmentsAsync(int id, CancellationToken cancellationToken = default) =>
        await dbSet
            .AsNoTracking()
            .Where(s => s.Id == id)
            .SelectMany(s => s.Appointments)
            .Select(a => a.Date)
            .Distinct()
            .ToArrayAsync(cancellationToken);
    
    public async Task<bool> IsServicesFromUniqueBarberShop(
        ServiceForUpdateAppointmentDtoRequest[] dtos,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        HashSet<int> ids = [..dtos.Select(s => s.Id)];
        
        return await dbSet
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.BarberShopId)
            .Distinct()
            .CountAsync(cancellationToken) == 1;
    }

    public async Task<Service[]> GetSpecificServicesByIdsAsync(
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var hashIds = ids.ToHashSet();
        return await dbSet.Where(x => hashIds.Contains(x.Id)).ToArrayAsync(cancellationToken);
    }
    
    public async Task<ServiceDtoResponse?> GetByIdAsync(
        int id, bool withAppointments = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var query = dbSet
            .AsNoTracking()
            .Where(s => s.Id == id);
            
        if (withAppointments)
        {
            query = query
                .AsSplitQuery()
                .Include(s => s.Appointments);
        }

        return await query
            .Select(s => new ServiceDtoResponse(
                s.Id,
                s.BarberShopId,
                s.BarberShop.Name,
                s.Name,
                s.Description,
                s.Price,
                s.Duration,
                s.Appointments
                    .Where(_ => withAppointments)
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
                        a.Status
                    ))
                    .ToArray()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<PaginationResponse<ServiceDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetAllAsync<ServiceDtoResponse>(
            new(
                page,
                pageSize,
                x => x.BarberShopId == barberShopId,
                new(x => x.Name),
                s => new(
                    s.Id,
                    s.BarberShopId,
                    s.BarberShop.Name,
                    s.Name,
                    s.Description,
                    s.Price,
                    s.Duration
                )
            ),
            cancellationToken
        );
    }
    
    public async Task<ServiceDtoResponse?> UpdateAsync(
        ServiceDtoRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var service = await dbSet.FindAsync([id], cancellationToken);
        
        if (service is null)
            return null;

        service.UpdateEntity(dto);

        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return service.CreateDto();
    }
    
    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var service = await dbSet.FindAsync([id], cancellationToken);
        
        if (service is null)
            return false;
            
        dbSet.Remove(service);
        return await SaveChangesAsync(cancellationToken);
    }
}
