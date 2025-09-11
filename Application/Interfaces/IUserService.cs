namespace ICorteApi.Application.Interfaces;

public interface IUserService : IService<User>
{
    Task<User?> CreateAsync(UserDtoRegisterCreate dtoRequest);
    Task<User?> GetMeAsync(bool? dispatchIncludes = null);
    Task<User> GetMyUserAsync();
    Task<int?> GetMyUserIdAsync();
    Task<UserRole[]> GetUserRolesAsync();
    Task<bool> AddUserRoleAsync(UserRole role);
    Task<bool> RemoveFromRoleAsync(UserRole role);
    Task<bool> UpdateEmailAsync(UserDtoEmailUpdate dtoRequest);
    Task<bool> UpdatePasswordAsync(UserDtoPasswordUpdate dtoRequest);
    Task<bool> UpdatePhoneNumberAsync(UserDtoPhoneNumberUpdate dtoRequest);
    Task<bool> DeleteAsync(int id);

    
    // Task<User?> CreateUserAsync(User newUser, string password);
    // Task<User?> GetMeAsync(bool? dispatchIncludes = null);
    // Task<int?> GetMyUserIdAsync();
    // Task<UserRole[]> GetUserRolesAsync();
    // Task<bool> AddUserRoleAsync(UserRole role);
    // Task<bool> RemoveFromRoleAsync(UserRole role);
    // Task<bool> UpdateEmailAsync(string newEmail);
    // Task<bool> UpdatePasswordAsync(string currentPassword, string newPassword);
    // Task<bool> UpdatePhoneNumberAsync(string newPhoneNumber);
    // Task<bool> DeleteAsync(User entity);
}
