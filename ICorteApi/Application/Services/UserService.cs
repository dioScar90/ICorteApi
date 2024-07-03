using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _repository = userRepository;
    
    public async Task<IResponseDataModel<User>> GetAsync()
    {
        return await _repository.GetAsync();
    }
    
    public async Task<int?> GetUserIdAsync()
    {
        return await _repository.GetUserIdAsync();
    }

    // public async Task<IResponseModel> UpdateAsync(int id, BarberShopDtoRequest dto)
    // {
    //     try
    //     {
    //         var result = await _repository.UpdateAsync(id, dto);
    //         return new ResponseModel { Success = result.Success };
    //     }
    //     catch (Exception)
    //     {
    //         throw;
    //     }
    // }
}
