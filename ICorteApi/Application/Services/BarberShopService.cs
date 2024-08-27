using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(IBarberShopRepository repository)
    : BaseService<BarberShop>(repository), IBarberShopService
{
    public async Task<ISingleResponse<BarberShop>> CreateAsync(IDtoRequest<BarberShop> dtoRequest, int ownerId)
    {
        if (dtoRequest is not BarberShopDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new BarberShop(dto, ownerId);
        return await CreateAsync(entity);
    }

    private async Task<ISingleResponse<BarberShop>> GetByIdAsync(int id, int ownerId, bool includes = false)
    {
        if (!includes)
            return await GetByIdAsync(id);
        
        return await GetByIdAsync(
            x => x.Id == id && x.OwnerId == ownerId,
            x => x.Address,
            x => x.RecurringSchedules,
            x => x.SpecialSchedules,
            x => x.Services);
    }

    public async Task<ISingleResponse<BarberShop>> GetByIdAsync(int id)
    {
        return await base.GetByIdAsync(id);
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<BarberShop> dtoRequest, int id, int ownerId)
    {
        var resp = await GetByIdAsync(id, ownerId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(int id, int ownerId)
    {
        var resp = await GetByIdAsync(id, ownerId, true);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
