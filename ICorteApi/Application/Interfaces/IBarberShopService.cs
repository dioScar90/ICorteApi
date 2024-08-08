using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService
    : IBasePrimaryKeyService<BarberShop, int>, IHasOneForeignKeyService<BarberShop, int>
{
    new Task<ISingleResponse<BarberShop>> CreateAsync(IDtoRequest<BarberShop> dto, int ownerId);
}
