using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService
    : IBasePrimaryKeyService<BarberShop, int>
{
    Task<int?> GetMyBarberShopAsync();
}
