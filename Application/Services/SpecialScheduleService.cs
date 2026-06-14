using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(
    AppDbContext context,
    UserService userService)
    : BaseService<SpecialSchedule>(context)
{
    public async Task<SpecialScheduleDtoResponse?> CreateAsync(
        SpecialScheduleDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var schedule = new SpecialSchedule(dto);

        dbSet.Add(schedule);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return schedule.CreateDto();
    }
    
    public async Task<EntityInfos> GetEntityInfosAsync(
        DateOnly date, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUserId = await userService.GetMyUserIdAsync();
        
        var infos = await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Date == date && x.BarberShopId == barberShopId)
            .Select(s => new EntityInfos(
                true,
                currentUserId != null && s.BarberShopId == currentUserId,
                s.BarberShopId == barberShopId
            ))
            .FirstOrDefaultAsync(cancellationToken);
            
        return infos ?? new();
    }
    
    public async Task<SpecialScheduleDtoResponse?> GetByIdAsync(
        DateOnly date, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<PaginationResponse<SpecialScheduleDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            ),
            cancellationToken
        );
    }

    public async Task<SpecialScheduleDtoResponse?> UpdateAsync(
        SpecialScheduleDtoRequest dto, DateOnly date, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var schedule = await dbSet.FindAsync([date, barberShopId], cancellationToken);

        if (schedule is null)
            return null;
            
        schedule.UpdateEntity(dto);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;
            
        return schedule.CreateDto();
    }

    public async Task<bool> DeleteAsync(
        DateOnly date, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var schedule = await dbSet.FindAsync([date, barberShopId], cancellationToken);

        if (schedule is null)
            return false;
            
        dbSet.Remove(schedule);
        return await SaveChangesAsync(cancellationToken);
    }
}
