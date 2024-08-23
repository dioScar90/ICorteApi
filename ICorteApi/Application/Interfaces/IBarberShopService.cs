using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService : IService<BarberShop>
{
    Task<ISingleResponse<BarberShop>> CreateAsync(IDtoRequest<BarberShop> dtoRequest, int ownerId);
    Task<ISingleResponse<BarberShop>> GetByIdAsync(int id);
    Task<IResponse> UpdateAsync(IDtoRequest<BarberShop> dtoRequest, int id, int ownerId);
    Task<IResponse> DeleteAsync(int id, int ownerId);
}
