using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IUserRepository
{
    Task<ISingleResponse<User>> GetAsync();
    Task<int?> GetUserIdAsync();
}
