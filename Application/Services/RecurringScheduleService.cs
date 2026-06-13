using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(
    AppDbContext context)
    : BaseService<RecurringSchedule>(context)
{
    public async Task<RecurringScheduleDtoResponse?> CreateAsync(
        RecurringScheduleDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var schedule = new RecurringSchedule(dto, dto.BarberShopId);

        dbSet.Add(schedule);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return schedule.CreateDto();
    }

    public async Task<bool> RecurringScheduleExists(
        DayOfWeek dayOfWeek, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.DayOfWeek == dayOfWeek && x.BarberShopId == barberShopId, cancellationToken);
    }
    
    public async Task<bool> RecurringScheduleBelongsToBarberShop(
        DayOfWeek dayOfWeek, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.DayOfWeek == dayOfWeek && x.BarberShopId == barberShopId, cancellationToken);
    }

    public async Task<RecurringScheduleDtoResponse?> GetByIdAsync(
        DayOfWeek dayOfWeek, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbSet
            .AsNoTracking()
            .Where(s => s.DayOfWeek == dayOfWeek && s.BarberShopId == barberShopId)
            .Select(s => new RecurringScheduleDtoResponse(
                s.DayOfWeek,
                s.BarberShopId,
                s.OpenTime,
                s.CloseTime,
                s.IsActive
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<PaginationResponse<RecurringScheduleDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            ),
            cancellationToken
        );
    }
    
    public async Task<bool> UpdateAsync(RecurringScheduleDtoRequest dto, DayOfWeek dayOfWeek, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var schedule = await dbSet.FindAsync([dayOfWeek, barberShopId], cancellationToken);

        if (schedule is null)
            return false;
            
        schedule.UpdateEntity(dto);
        return await SaveChangesAsync(cancellationToken);
    }
    
    public async Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var schedule = await dbSet.FindAsync([dayOfWeek, barberShopId], cancellationToken);

        if (schedule is null)
            return false;
            
        dbSet.Remove(schedule);
        return await SaveChangesAsync(cancellationToken);
    }
}
