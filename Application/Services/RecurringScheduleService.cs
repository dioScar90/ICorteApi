using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(
    AppDbContext context)
    : BaseService<RecurringSchedule>(context)
{
    public async Task<RecurringScheduleDtoResponse?> CreateAsync(RecurringScheduleDtoRequest dto)
    {
        var schedule = new RecurringSchedule(dto, dto.BarberShopId);

        _dbSet.Add(schedule);
        await SaveChangesAsync();

        return await GetByIdAsync(schedule.DayOfWeek, schedule.BarberShopId);
    }

    public async Task<bool> RecurringScheduleExists(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.DayOfWeek == dayOfWeek && x.BarberShopId == barberShopId);
    }
    
    public async Task<bool> RecurringScheduleBelongsToBarberShop(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.DayOfWeek == dayOfWeek && x.BarberShopId == barberShopId);
    }

    public async Task<RecurringScheduleDtoResponse?> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.DayOfWeek == dayOfWeek && s.BarberShopId == barberShopId)
            .Select(s => new RecurringScheduleDtoResponse(
                s.DayOfWeek,
                s.BarberShopId,
                s.OpenTime,
                s.CloseTime,
                s.IsActive
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<PaginationResponse<RecurringScheduleDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync<RecurringScheduleDtoResponse>(
            new(
                page,
                pageSize,
                x => x.BarberShopId == barberShopId,
                new(x => x.DayOfWeek),
                s => new(
                    s.DayOfWeek,
                    s.BarberShopId,
                    s.OpenTime,
                    s.CloseTime,
                    s.IsActive
                )
            )
        );
    }
    
    public async Task<bool> UpdateAsync(RecurringScheduleDtoRequest dto, DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await _dbSet.FindAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            return false;
            
        schedule.UpdateEntity(dto);
        return await SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await _dbSet.FindAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            return false;
            
        _dbSet.Remove(schedule);
        return await SaveChangesAsync();
    }
}
