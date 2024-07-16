using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService
{
       Task<IResponse> CreateAsync(BarberShop barberShop);
       Task<ISingleResponse<BarberShop>> GetByIdAsync(int id);
       Task<ICollectionResponse<BarberShop>> GetAllAsync(int page, int pageSize);
       Task<IResponse> UpdateAsync(int id, BarberShopDtoRequest dto);
       Task<IResponse> DeleteAsync(int id);
}
