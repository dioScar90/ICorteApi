using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IUserRepository : IRepository<User>
{
    int? GetMyUserId();
    Task<UserRole[]> GetUserRolesAsync();
    Task<IResponse> AddUserRoleAsync(UserRole role);
    Task<IResponse> RemoveUserRoleAsync(UserRole role);
    Task<ISingleResponse<User>> GetMeAsync();
    Task<IResponse> UpdateAsync(User entity);
    Task<IResponse> DeleteAsync(User entity);
}
