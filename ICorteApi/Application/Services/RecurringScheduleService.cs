using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(IRecurringScheduleRepository repository)
    : BaseCompositeKeyService<RecurringSchedule, DayOfWeek, int>(repository), IRecurringScheduleService
{
    public async Task<ISingleResponse<RecurringSchedule>> CreateAsync(IDtoRequest<RecurringSchedule> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not RecurringScheduleDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new RecurringSchedule(dto, barberShopId);
        return await CreateByEntityAsync(entity);
    }

    public override async Task<ISingleResponse<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _repository.GetByIdAsync(x => x.DayOfWeek == dayOfWeek && x.BarberShopId == barberShopId);
    }

    public override async Task<IResponse> UpdateAsync(IDtoRequest<RecurringSchedule> dtoRequest, DayOfWeek dayOfWeek, int barberShopId)
    {
        var resp = await GetByIdAsync(dayOfWeek, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await _repository.UpdateAsync(entity);
    }

    public override async Task<IResponse> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var resp = await GetByIdAsync(dayOfWeek, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await _repository.DeleteAsync(entity);
    }
}
