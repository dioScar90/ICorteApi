using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
    : IBasePrimaryKeyRepository<BarberShop, int>
{
}
