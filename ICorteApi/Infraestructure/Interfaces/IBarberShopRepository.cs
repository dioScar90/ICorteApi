using System.Linq.Expressions;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository : IBaseRepository
{
    Task<IResponseModel> CreateAsync(BarberShop barberShop);
    Task<IResponseDataModel<BarberShop>> GetAsync(Expression<Func<BarberShop, bool>>? filter);
    Task<IResponseDataModel<IEnumerable<BarberShop>>> GetAllAsync(Expression<Func<BarberShop, bool>>? filter = null);
    Task<IResponseModel> UpdateAsync(BarberShop barberShop);
    Task<IResponseModel> DeleteAsync(int id);
}
