using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(IRecurringScheduleRepository repository)
    : BaseService<RecurringSchedule>(repository), IRecurringScheduleService
{
    public async Task<ISingleResponse<RecurringSchedule>> CreateAsync(IDtoRequest<RecurringSchedule> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not RecurringScheduleDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new RecurringSchedule(dto, barberShopId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await base.GetByIdAsync(dayOfWeek, barberShopId);
    }
    
    public async Task<ICollectionResponse<RecurringSchedule>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(
            new(
                page, pageSize,
                x => x.BarberShopId == barberShopId,
                false, x => x.DayOfWeek
            )
        );
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<RecurringSchedule> dtoRequest, DayOfWeek dayOfWeek, int barberShopId)
    {
        var resp = await GetByIdAsync(dayOfWeek, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var resp = await GetByIdAsync(dayOfWeek, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
