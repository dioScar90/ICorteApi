using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(ISpecialScheduleRepository repository)
    : BaseCompositeKeyService<SpecialSchedule, DateOnly, int>(repository), ISpecialScheduleService
{
    public async Task<ISingleResponse<SpecialSchedule>> CreateAsync(IDtoRequest<SpecialSchedule> dto, int barberShopId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.BarberShopId = barberShopId;

        return await CreateByEntityAsync(entity);
    }

    public override async Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return await _repository.GetByIdAsync(x => x.Date == date && x.BarberShopId == barberShopId);
    }

    public override async Task<IResponse> UpdateAsync(IDtoRequest<SpecialSchedule> dto, DateOnly date, int barberShopId)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);

        return await _repository.UpdateAsync(entity);
    }

    public override async Task<IResponse> DeleteAsync(DateOnly date, int barberShopId)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await _repository.DeleteAsync(entity);
    }
}
