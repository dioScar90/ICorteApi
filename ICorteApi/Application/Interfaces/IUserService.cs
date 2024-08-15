using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IUserService : IService<User>, IHasNoForeignKeyService<User>
{
    Task<ISingleResponse<User>> GetMeAsync();
    Task<User> GetMyUserAsync();
    int GetMyUserId();
    Task<UserRole[]> GetUserRolesAsync();
    Task<IResponse> AddUserRoleAsync(UserRole role);
    Task<IResponse> RemoveFromRoleAsync(UserRole role);
    Task<IResponse> UpdateEmailAsync(UserDtoChangeEmailRequest dtoRequest);
    Task<IResponse> UpdatePasswordAsync(UserDtoChangePasswordRequest dtoRequest);
    Task<IResponse> UpdatePhoneNumberAsync(UserDtoChangePhoneNumberRequest dtoRequest);
    Task<IResponse> DeleteAsync(int id);
}
