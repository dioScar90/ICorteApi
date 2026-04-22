using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(
    AppDbContext context)
    : BaseService<SpecialSchedule>(context)
{
    public async Task<SpecialScheduleDtoResponse?> CreateAsync(SpecialScheduleDtoRequest dto)
    {
        var schedule = new SpecialSchedule(dto);

        dbSet.Add(schedule);
        
        if (!await SaveChangesAsync())
            return null;

        return schedule.CreateDto();
    }

    public async Task<bool> SpecialScheduleExists(DateOnly date, int barberShopId)
    {
        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Date == date && x.BarberShopId == barberShopId);
    }
    
    public async Task<bool> SpecialScheduleBelongsToBarberShop(DateOnly date, int barberShopId)
    {
        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Date == date && x.BarberShopId == barberShopId);
    }

    public async Task<SpecialScheduleDtoResponse?> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return await dbSet
            .AsNoTracking()
            .Where(s => s.Date == date && s.BarberShopId == barberShopId)
            .Select(s => new SpecialScheduleDtoResponse(
                s.Date,
                s.BarberShopId,
                s.DayOfWeek,
                s.Notes,
                s.OpenTime,
                s.CloseTime,
                s.IsClosed
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<PaginationResponse<SpecialScheduleDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync<SpecialScheduleDtoResponse>(
            new(
                page,
                pageSize,
                x => x.BarberShopId == barberShopId,
                new(x => x.Date),
                s => new(
                    s.Date,
                    s.BarberShopId,
                    s.DayOfWeek,
                    s.Notes,
                    s.OpenTime,
                    s.CloseTime,
                    s.IsClosed
                )
            )
        );
    }

    public async Task<bool> UpdateAsync(SpecialScheduleDtoRequest dto, DateOnly date, int barberShopId)
    {
        var schedule = await dbSet.FindAsync(date, barberShopId);

        if (schedule is null)
            return false;
            
        schedule.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(DateOnly date, int barberShopId)
    {
        var schedule = await dbSet.FindAsync(date, barberShopId);

        if (schedule is null)
            return false;
            
        dbSet.Remove(schedule);
        return await SaveChangesAsync();
    }
}
