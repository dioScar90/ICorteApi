using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<ISingleResponse<User>> CreateUserAsync(User newUser, string password);
    Task<ISingleResponse<User>> GetMeAsync();
    int? GetMyUserId();
    Task<UserRole[]> GetUserRolesAsync();
    Task<IResponse> AddUserRoleAsync(UserRole role);
    Task<IResponse> RemoveFromRoleAsync(UserRole role);
    Task<IResponse> UpdateEmailAsync(string newEmail);
    Task<IResponse> UpdatePasswordAsync(string currentPassword, string newPassword);
    Task<IResponse> UpdatePhoneNumberAsync(string newPhoneNumber);
    Task<IResponse> DeleteAsync(User entity);
}
