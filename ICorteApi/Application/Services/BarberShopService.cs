using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(IBarberShopRepository repository, IBarberShopErrors errors)
    : BaseService<BarberShop>(repository), IBarberShopService
{
    private readonly IBarberShopErrors _errors = errors;

    public async Task<BarberShop?> CreateAsync(BarberShopDtoRequest dto, int ownerId)
    {
        var barberShop = new BarberShop(dto, ownerId);
        return await CreateAsync(barberShop);
    }

    private async Task<BarberShop?> GetByIdAsync(int id, int ownerId, bool includes = false)
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

    public async Task<BarberShop?> GetByIdAsync(int id)
    {
        return await base.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(BarberShopDtoRequest dto, int id, int ownerId)
    {
        var barberShop = await GetByIdAsync(id);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        barberShop!.UpdateEntityByDto(dto);
        return await UpdateAsync(barberShop);
    }

    public async Task<bool> DeleteAsync(int id, int ownerId)
    {
        var barberShop = await GetByIdAsync(id, ownerId, true);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        return await DeleteAsync(barberShop);
    }
}
