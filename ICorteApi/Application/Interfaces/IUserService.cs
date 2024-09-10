using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Interfaces;

public interface IUserService : IService<User>
{
    Task<User?> CreateAsync(UserDtoRegisterCreate dtoRequest);
    Task<User?> GetMeAsync();
    Task<User> GetMyUserAsync();
    Task<int> GetMyUserIdAsync();
    Task<UserRole[]> GetUserRolesAsync();
    Task<bool> AddUserRoleAsync(UserRole role);
    Task<bool> RemoveFromRoleAsync(UserRole role);
    Task<bool> UpdateEmailAsync(UserDtoEmailUpdate dtoRequest);
    Task<bool> UpdatePasswordAsync(UserDtoPasswordUpdate dtoRequest);
    Task<bool> UpdatePhoneNumberAsync(UserDtoPhoneNumberUpdate dtoRequest);
    Task<bool> DeleteAsync(int id);
}
