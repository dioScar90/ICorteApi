using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService
    : IBasePrimaryKeyService<BarberShop, int>
{
    Task<ISingleResponse<BarberShop>> CreateAsync(int ownerId, BarberShopDtoRequest dto);
}
