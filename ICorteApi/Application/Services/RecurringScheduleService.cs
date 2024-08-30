using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(IRecurringScheduleRepository repository, IRecurringScheduleErrors errors)
    : BaseService<RecurringSchedule>(repository), IRecurringScheduleService
{
    private readonly IRecurringScheduleErrors _errors = errors;

    public async Task<RecurringSchedule?> CreateAsync(RecurringScheduleDtoRequest dto, int barberShopId)
    {
        var schedule = new RecurringSchedule(dto, barberShopId);
        return await CreateAsync(schedule);
    }

    public async Task<RecurringSchedule?> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await base.GetByIdAsync(dayOfWeek, barberShopId);
    }
    
    public async Task<RecurringSchedule[]> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(
            new(
                page, pageSize,
                x => x.BarberShopId == barberShopId,
                false, x => x.DayOfWeek
            )
        );
    }
    
    public async Task<bool> UpdateAsync(RecurringScheduleDtoRequest dtoRequest, DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowRecurringScheduleNotBelongsToBarberShopException(barberShopId);

        schedule.UpdateEntityByDto(dtoRequest);
        return await UpdateAsync(schedule);
    }

    public async Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowRecurringScheduleNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(schedule);
    }
}
