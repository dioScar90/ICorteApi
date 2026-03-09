using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(
    AppDbContext context,
    UserService _userService)
    : BaseService<Service>(context)
{
    public async Task<ServiceDtoResponse?> CreateAsync(ServiceDtoRequest dto)
    {
        var service = new Service(dto, dto.BarberShopId);
        
        _dbSet.Add(service);
        
        if (!await SaveChangesAsync())
            return null;

        return service.CreateDto();
    }
    
    public async Task<bool> ServiceExists(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }
    
    public async Task<bool> ServiceBelongsToBarberShop(int id, int? barberShopId = null)
    {
        barberShopId ??= await _userService.GetMyUserIdAsync();

        return await _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && x.BarberShopId == barberShopId);
    }

    public async Task<bool> CheckCorrelatedAppointmentsAsync(int id) =>
        await _dbSet
            .AsNoTracking()
            .AnyAsync(s => s.Id == id && s.Appointments.Any());
            
    public async Task<DateOnly[]> GetDatesFromCorrelatedAppointmentsAsync(int id) =>
        await _dbSet
            .AsNoTracking()
            .Where(s => s.Id == id)
            .SelectMany(s => s.Appointments)
            .Select(a => a.Date)
            .Distinct()
            .ToArrayAsync();
    
    public async Task<bool> IsServicesFromUniqueBarberShop(ServiceForUpdateAppointmentDtoRequest[] dtos)
    {
        HashSet<int> ids = [..dtos.Select(s => s.Id)];
        
        return await _dbSet
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.BarberShopId)
            .Distinct()
            .CountAsync() == 1;
    }

    public async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        var hashIds = ids.ToHashSet();
        return await _dbSet.Where(x => hashIds.Contains(x.Id)).ToArrayAsync();
    }
    
    public record Includes(bool Collections = false);
    
    public async Task<ServiceDtoResponse?> GetByIdAsync(int id, int barberShopId, Includes? includes = null)
    {
        includes ??= new();

        var query = _dbSet
            .AsNoTracking()
            .Where(s => s.Id == id);
            
        if (includes.Collections)
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
                s.Duration
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<PaginationResponse<ServiceDtoResponse>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
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
            )
        );
    }
    
    public async Task<bool> UpdateAsync(ServiceDtoRequest dto, int id, int barberShopId)
    {
        var service = await _dbSet.FindAsync(id);
        
        if (service is null)
            return false;

        service.UpdateEntity(dto);
        return await SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var service = await _dbSet.FindAsync(id);
        
        if (service is null)
            return false;
            
        _dbSet.Remove(service);
        return await SaveChangesAsync();
    }
}
