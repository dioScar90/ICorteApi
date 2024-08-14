using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(ISpecialScheduleRepository repository)
    : BaseCompositeKeyService<SpecialSchedule, DateOnly, int>(repository), ISpecialScheduleService
{
    public async Task<ISingleResponse<SpecialSchedule>> CreateAsync(IDtoRequest<SpecialSchedule> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not SpecialScheduleDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new SpecialSchedule(dto, barberShopId);
        return await CreateByEntityAsync(entity);
    }

    public override async Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return await _repository.GetByIdAsync(x => x.Date == date && x.BarberShopId == barberShopId);
    }

    public override async Task<IResponse> UpdateAsync(IDtoRequest<SpecialSchedule> dtoRequest, DateOnly date, int barberShopId)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

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
