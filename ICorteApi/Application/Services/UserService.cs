using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository = repository;

    public async Task<ISingleResponse<User>> GetMeAsync()
    {
        return await _repository.GetMeAsync();
    }

    public async Task<User> GetMyUserAsync() => (await GetMeAsync()).Value!;

    public int GetMyUserId()
    {
        return (int)_repository.GetMyUserId()!;
    }

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        return await _repository.GetUserRolesAsync();
    }

    public async Task<IResponse> AddUserRoleAsync(UserRole role)
    {
        return await _repository.AddUserRoleAsync(role);
    }

    public async Task<IResponse> RemoveFromRoleAsync(UserRole role)
    {
        return await _repository.RemoveFromRoleAsync(role);
    }
    
    public async Task<IResponse> UpdateEmailAsync(UserDtoChangeEmailRequest dtoRequest)
    {
        return await _repository.UpdateEmailAsync(dtoRequest.Email);
    }
    
    public async Task<IResponse> UpdatePasswordAsync(UserDtoChangePasswordRequest dtoRequest)
    {
        return await _repository.UpdatePasswordAsync(dtoRequest.CurrentPassword, dtoRequest.NewPassword);
    }

    public async Task<IResponse> UpdatePhoneNumberAsync(UserDtoChangePhoneNumberRequest dtoRequest)
    {
        return await _repository.UpdatePhoneNumberAsync(dtoRequest.PhoneNumber);
    }

    public async Task<IResponse> DeleteAsync(int id)
    {
        var resp = await _repository.GetMeAsync();

        if (!resp.IsSuccess)
            return resp;

        var user = resp.Value!;

        if (user.Id != id)
            return Response.Failure(Error.UserNotFound);

        return await _repository.DeleteAsync(user);
    }

    public async Task<ISingleResponse<User>> CreateAsync(IDtoRequest<User> dtoRequest)
    {
        if (dtoRequest is not UserDtoRegisterRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new User(dto);
        return await _repository.CreateUserAsync(entity, dto.Password);
    }
}
