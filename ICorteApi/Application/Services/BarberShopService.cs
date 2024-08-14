using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(IBarberShopRepository repository)
    : BasePrimaryKeyService<BarberShop, int>(repository), IBarberShopService
{
    public async Task<ISingleResponse<BarberShop>> CreateAsync(IDtoRequest<BarberShop> dtoRequest, int ownerId)
    {
        if (dtoRequest is not BarberShopDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new BarberShop(dto, ownerId);
        return await CreateByEntityAsync(entity);
    }
}
