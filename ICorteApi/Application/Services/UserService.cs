using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class UserService(IUserRepository repository, IUserErrors errors) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IUserErrors _errors = errors;

    public async Task<User?> CreateAsync(UserDtoRegisterRequest dto)
    {
        var user = new User(dto);
        return await _repository.CreateUserAsync(user, user.GetPasswordToBeHashed());
    }

    public async Task<User?> GetMeAsync()
    {
        return await _repository.GetMeAsync();
    }

    public async Task<User> GetMyUserAsync() => (await GetMeAsync())!;

    public int GetMyUserId()
    {
        return (int)_repository.GetMyUserId()!;
    }

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        return await _repository.GetUserRolesAsync();
    }

    public async Task<bool> AddUserRoleAsync(UserRole role)
    {
        return await _repository.AddUserRoleAsync(role);
    }

    public async Task<bool> RemoveFromRoleAsync(UserRole role)
    {
        return await _repository.RemoveFromRoleAsync(role);
    }
    
    public async Task<bool> UpdateEmailAsync(UserDtoChangeEmailRequest dtoRequest)
    {
        return await _repository.UpdateEmailAsync(dtoRequest.Email);
    }
    
    public async Task<bool> UpdatePasswordAsync(UserDtoChangePasswordRequest dtoRequest)
    {
        return await _repository.UpdatePasswordAsync(dtoRequest.CurrentPassword, dtoRequest.NewPassword);
    }

    public async Task<bool> UpdatePhoneNumberAsync(UserDtoChangePhoneNumberRequest dtoRequest)
    {
        return await _repository.UpdatePhoneNumberAsync(dtoRequest.PhoneNumber);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _repository.GetMeAsync();

        if (user is null)
            _errors.ThrowNotFoundException();

        if (user!.Id != id)
            _errors.ThrowWrongUserIdException(id);
        
        return await _repository.DeleteAsync(user);
    }
}
