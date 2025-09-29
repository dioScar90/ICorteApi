using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(
    AppDbContext context,
    ILogger<RecurringScheduleService> logger,
    RecurringScheduleValidator validator,
    RecurringScheduleErrors errors)
    : BaseService<RecurringSchedule>(context, logger)
{
    private readonly RecurringScheduleValidator _validator = validator;
    private readonly RecurringScheduleErrors _errors = errors;

    public async Task<RecurringScheduleDtoResponse> CreateAsync(RecurringScheduleDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors, _logger);

        var schedule = new RecurringSchedule(dto, dto.BarberShopId);

        _dbSet.Add(schedule);
        await SaveChangesAsync();

        return await GetByIdAsync(schedule.DayOfWeek, schedule.BarberShopId);
    }

    public async Task<RecurringScheduleDtoResponse> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await _dbSet
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

        if (schedule is null)
            _errors.ThrowNotFoundException();
        
        return schedule!;
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
        dto.ThrowExceptionIfInvalid(_validator, _errors, _logger);

        var schedule = await _dbSet.FindAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowRecurringScheduleNotBelongsToBarberShopException(barberShopId);

        schedule.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await _dbSet.FindAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowRecurringScheduleNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(schedule);
    }
}
