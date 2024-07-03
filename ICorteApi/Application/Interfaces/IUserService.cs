using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IUserService
{
       Task<IResponseDataModel<User>> GetAsync();
       Task<int?> GetUserIdAsync();
       // Task<IResponseModel> UpdateAsync(int userId, PersonDtoRequest dto);
}
