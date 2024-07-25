using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IUserService
{
    Task<ISingleResponse<User>> GetAsync();
    Task<int?> GetUserIdAsync();
}
