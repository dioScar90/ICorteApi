using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class BarberShopService(IBarberShopRepository barberShopRepository, IUserService userService)
    : BasePrimaryKeyService<BarberShop, int>(barberShopRepository), IBarberShopService
{
    private readonly IUserService _userService = userService;
    
    public async Task<int?> GetMyBarberShopAsync() => await _userService.GetUserIdAsync();
}
