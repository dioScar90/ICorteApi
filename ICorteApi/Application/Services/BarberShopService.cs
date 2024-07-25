using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class BarberShopService(IBarberShopRepository barberShopRepository, IUserService userService)
    : BasePrimaryKeyService<BarberShop, int>(barberShopRepository), IBarberShopService
{
    private readonly IBarberShopRepository _primaryBarberShopRepository = barberShopRepository;
    private readonly IUserService _userService = userService;
    
    public async Task<int?> GetMyBarberShopAsync() => await _userService.GetUserIdAsync();

    public async Task<IResponse> CreateAsync(int ownerId, BarberShopDtoRequest dto)
    {
        var newBarberShop = dto.CreateEntity()!;
        newBarberShop.OwnerId = ownerId;

        return await _primaryBarberShopRepository.CreateAsync(newBarberShop);
    }
}
