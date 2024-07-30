using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class SpecialScheduleService(ISpecialScheduleRepository specialScheduleRepository)
    : BaseCompositeKeyService<SpecialSchedule, DateOnly, int>(specialScheduleRepository), ISpecialScheduleService
{
    public override async Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return await _repository.GetByIdAsync(x => x.Date == date && x.BarberShopId == barberShopId);
    }
    
    public override async Task<IResponse> UpdateAsync(DateOnly date, int barberShopId, IDtoRequest<SpecialSchedule> dto)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        entity.UpdateEntityByDto(dto);
        
        return await UpdateEntityAsync(entity);
    }

    public override async Task<IResponse> DeleteAsync(DateOnly date, int barberShopId)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;
        
        var entity = resp.Value!;
        return await DeleteEntityAsync(entity);
    }
}
