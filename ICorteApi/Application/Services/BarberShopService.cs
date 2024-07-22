using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class BarberShopService(IBarberShopRepository barberShopRepository, IUserService userService) : IBarberShopService
{
    private readonly IBarberShopRepository _repository = barberShopRepository;
    private readonly IUserService _userService = userService;

    public async Task<IResponse> CreateAsync(BarberShop barberShop)
    {
        return await _repository.CreateAsync(barberShop);
    }

    public async Task<IResponse> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<ICollectionResponse<BarberShop>> GetAllAsync(int page, int pageSize)
    {
        return await _repository.GetAllAsync(1, 25);
    }

    public async Task<ISingleResponse<BarberShop>> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<ISingleResponse<BarberShop>> GetMyBarberShopAsync()
    {
        var myUserId = await _userService.GetUserIdAsync();

        if (myUserId is null)
            return Response.Failure<BarberShop>(Error.UserNotFound);
        
        return await _repository.GetMyBarberShopAsync((int)myUserId);
    }

    public async Task<IResponse> UpdateAsync(int id, BarberShopDtoRequest dto)
    {
        var response = await _repository.GetByIdAsync(id);

        if (!response.IsSuccess)
            return response; // "Barbearia não encontrada"
        
        var barberShop = response.Value!;

        barberShop.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(barberShop);
    }
}
