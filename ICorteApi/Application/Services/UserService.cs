using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository = repository;

    public async Task<ISingleResponse<User>> GetMeAsync()
    {
        return await _repository.GetMeAsync();
    }

    public int? GetMyUserIdAsync()
    {
        return _repository.GetMyUserId();
    }

    public async Task<UserRole[]> GetUserRolesAsync()
    {
        return await _repository.GetUserRolesAsync();
    }

    public async Task<IResponse> AddUserRoleAsync(UserRole role, int id)
    {
        var resp = await _repository.GetMeAsync();

        if (!resp.IsSuccess)
            return resp;

        var user = resp.Value!;

        if (user.Id != id)
            return Response.Failure(Error.UserNotFound);

        return await _repository.AddUserRoleAsync(role);
    }

    public async Task<IResponse> RemoveUserRoleAsync(UserRole role, int id)
    {
        var resp = await _repository.GetMeAsync();

        if (!resp.IsSuccess)
            return resp;

        var user = resp.Value!;

        if (user.Id != id)
            return Response.Failure(Error.UserNotFound);

        return await _repository.RemoveUserRoleAsync(role);
    }

    public async Task<IResponse> UpdateAsync(UserDtoRequest dto, int id)
    {
        var resp = await _repository.GetMeAsync();

        if (!resp.IsSuccess)
            return resp;

        var user = resp.Value!;

        if (user.Id != id)
            return Response.Failure(Error.UserNotFound);
            
        user.IsRegisterCompleted = user.CheckRegisterCompletation();
        user.UpdateEntityByDto(dto);

        return await _repository.UpdateAsync(user);
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

    public Task<ISingleResponse<User>> CreateAsync(IDtoRequest<User> dto)
    {
        throw new NotImplementedException();
    }
}
