using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> CreateUserAsync(User newUser, string password);
    Task<User?> GetMeAsync();
    int? GetMyUserId();
    Task<UserRole[]> GetUserRolesAsync();
    Task<bool> AddUserRoleAsync(UserRole role);
    Task<bool> RemoveFromRoleAsync(UserRole role);
    Task<bool> UpdateEmailAsync(string newEmail);
    Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword);
    Task<bool> UpdatePhoneNumberAsync(string newPhoneNumber);
    Task<bool> DeleteAsync(User entity);
}
