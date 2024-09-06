using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Interfaces;

public interface IUserService : IService<User>
{
    Task<User?> CreateAsync(UserDtoRegisterRequest dtoRequest);
    Task<User?> GetMeAsync();
    Task<User> GetMyUserAsync();
    Task<int> GetMyUserIdAsync();
    Task<UserRole[]> GetUserRolesAsync();
    Task<bool> AddUserRoleAsync(UserRole role);
    Task<bool> RemoveFromRoleAsync(UserRole role);
    Task<bool> UpdateEmailAsync(UserDtoChangeEmailRequest dtoRequest);
    Task<bool> UpdatePasswordAsync(UserDtoChangePasswordRequest dtoRequest);
    Task<bool> UpdatePhoneNumberAsync(UserDtoChangePhoneNumberRequest dtoRequest);
    Task<bool> DeleteAsync(int id);
}
