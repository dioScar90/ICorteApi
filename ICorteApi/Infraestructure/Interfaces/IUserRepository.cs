using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IUserRepository
{
    Task<IResponseDataModel<User>> GetAsync();
    Task<int?> GetUserIdAsync();
    // Task<IResponseModel> UpdateAsync(int userId, PersonDtoRequest dto);
}
