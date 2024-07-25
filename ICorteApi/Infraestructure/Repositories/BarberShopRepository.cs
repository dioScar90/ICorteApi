using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public class BarberShopRepository(AppDbContext context)
    : BasePrimaryKeyRepository<BarberShop, int>(context), IBarberShopRepository
{
}
