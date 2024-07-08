using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IPersonService
{
       Task<IResponseModel> CreateAsync(Person person);
       Task<IResponseDataModel<Person>> GetByIdAsync(int userId);
       Task<IResponseDataModel<BarberShop>> GetMyBarberShopAsync();
       Task<IResponseDataModel<ICollection<Person>>> GetAllAsync(int page, int pageSize);
       Task<IResponseModel> UpdateAsync(int userId, PersonDtoRequest dto);
       Task<IResponseModel> DeleteAsync(int userId);
}
