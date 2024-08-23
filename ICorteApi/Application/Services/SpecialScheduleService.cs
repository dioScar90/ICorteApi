using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(ISpecialScheduleRepository repository)
    : BaseService<SpecialSchedule>(repository), ISpecialScheduleService
{
    public async Task<ISingleResponse<SpecialSchedule>> CreateAsync(IDtoRequest<SpecialSchedule> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not SpecialScheduleDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new SpecialSchedule(dto, barberShopId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return await GetByIdAsync(x => x.Date == date && x.BarberShopId == barberShopId);
    }
    
    public async Task<ICollectionResponse<SpecialSchedule>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
    }

    public async Task<IResponse> UpdateAsync(IDtoRequest<SpecialSchedule> dtoRequest, DateOnly date, int barberShopId)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(DateOnly date, int barberShopId)
    {
        var resp = await GetByIdAsync(date, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
