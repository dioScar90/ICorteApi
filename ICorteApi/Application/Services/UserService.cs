using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _repository = userRepository;
    
    public async Task<ISingleResponse<User>> GetAsync()
    {
        return await _repository.GetAsync();
    }
    
    public async Task<int?> GetUserIdAsync()
    {
        return await _repository.GetUserIdAsync();
    }
}
