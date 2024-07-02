using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService
{
       Task<IResponseModel> CreateAsync(BarberShop barberShop);
       Task<IResponseDataModel<BarberShop>> GetByIdAsync(int id);
       Task<IResponseDataModel<IEnumerable<BarberShop>>> GetAllAsync(int page, int pageSize);
       Task<IResponseModel> UpdateAsync(int id, BarberShopDtoRequest dto);
       Task<IResponseModel> DeleteAsync(int id);
}
