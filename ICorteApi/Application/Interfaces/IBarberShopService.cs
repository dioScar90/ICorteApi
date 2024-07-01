using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService
{
       Task<IResponseModel> CreateAsync(BarberShop barberShop);
       Task<IResponseDataModel<BarberShop>> GetByIdAsync(int id);
       Task<IResponseDataModel<IEnumerable<BarberShop>>> GetAllAsync();
       Task<IResponseModel> UpdateAsync(BarberShop barberShop);
       Task<IResponseModel> DeleteAsync(int id);
}
