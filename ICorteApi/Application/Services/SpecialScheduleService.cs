using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(ISpecialScheduleRepository repository, ISpecialScheduleErrors errors)
    : BaseService<SpecialSchedule>(repository), ISpecialScheduleService
{
    private readonly ISpecialScheduleErrors _errors = errors;

    public async Task<SpecialSchedule?> CreateAsync(SpecialScheduleDtoRequest dto, int barberShopId)
    {
        var schedule = new SpecialSchedule(dto, barberShopId);
        return await CreateAsync(schedule);
    }

    public async Task<SpecialSchedule?> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return await base.GetByIdAsync(date, barberShopId);
    }
    
    public async Task<SpecialSchedule[]> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
    }

    public async Task<bool> UpdateAsync(SpecialScheduleDtoRequest dto, DateOnly date, int barberShopId)
    {
        var schedule = await GetByIdAsync(date, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowSpecialScheduleNotBelongsToBarberShopException(barberShopId);
        
        schedule.UpdateEntityByDto(dto);
        return await UpdateAsync(schedule);
    }

    public async Task<bool> DeleteAsync(DateOnly date, int barberShopId)
    {
        var schedule = await GetByIdAsync(date, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowSpecialScheduleNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(schedule);
    }
}
