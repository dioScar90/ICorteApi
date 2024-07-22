using System.Linq.Expressions;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
{
    Task<IResponse> CreateAsync(BarberShop barberShop);
    Task<ISingleResponse<BarberShop>> GetByIdAsync(int id);
    Task<ICollectionResponse<BarberShop>> GetAllAsync(int page, int pageSize, Expression<Func<BarberShop, bool>>? filter = null);
    Task<ISingleResponse<BarberShop>> GetMyBarberShopAsync(int ownerId);
    Task<IResponse> UpdateAsync(BarberShop barberShop);
    Task<IResponse> DeleteAsync(int id);
}
