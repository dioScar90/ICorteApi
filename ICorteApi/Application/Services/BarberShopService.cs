using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(IBarberShopRepository repository)
    : BasePrimaryKeyService<BarberShop, int>(repository), IBarberShopService
{
    private readonly IBarberShopRepository _primaryBarberShopRepository = repository;
    
    public async Task<ISingleResponse<BarberShop>> CreateAsync(int ownerId, BarberShopDtoRequest dto)
    {
        var newBarberShop = dto.CreateEntity()!;
        newBarberShop.OwnerId = ownerId;

        return await _primaryBarberShopRepository.CreateAsync(newBarberShop);
    }
}
