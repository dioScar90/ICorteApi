using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(
    AppDbContext context,
    ILogger<SpecialScheduleService> _logger,
    SpecialScheduleErrors _errors)
    : BaseService<SpecialSchedule>(context)
{
    public async Task<SpecialScheduleDtoResponse> CreateAsync(SpecialScheduleDtoRequest dto)
    {
        var schedule = new SpecialSchedule(dto);

        _dbSet.Add(schedule);
        await SaveChangesAsync();

        return await GetByIdAsync(schedule.Date, schedule.BarberShopId);
    }

    public async Task<SpecialScheduleDtoResponse> GetByIdAsync(DateOnly date, int barberShopId)
    {
        var schedule = await _dbSet
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

        if (schedule is null)
            _errors.ThrowNotFoundException();

        return schedule!;
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

    public async Task UpdateAsync(SpecialScheduleDtoRequest dto, DateOnly date, int barberShopId)
    {
        var schedule = await _dbSet.FindAsync(date, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowSpecialScheduleNotBelongsToBarberShopException(barberShopId);

        schedule.UpdateEntity(dto);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(DateOnly date, int barberShopId)
    {
        var schedule = await _dbSet.FindAsync(date, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowSpecialScheduleNotBelongsToBarberShopException(barberShopId);
        
        await DeleteAsync(schedule);
    }
}
