using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IUserService : IService<User>, IHasNoForeignKeyService<User>
{
    Task<ISingleResponse<User>> GetMeAsync();
    int? GetMyUserIdAsync();
    Task<UserRole[]> GetUserRolesAsync();
    Task<IResponse> AddUserRoleAsync(UserRole role, int id);
    Task<IResponse> RemoveUserRoleAsync(UserRole role, int id);
    Task<IResponse> UpdateAsync(UserDtoRequest dtoRequest, int id);
    Task<IResponse> DeleteAsync(int id);
}
