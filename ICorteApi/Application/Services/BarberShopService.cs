using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class BarberShopService(IBarberShopRepository barberShopRepository) : IBarberShopService
{
    private readonly IBarberShopRepository _repository = barberShopRepository;

    public async Task<IResponseModel> CreateAsync(BarberShop barberShop)
    {
        try
        {
            var result = await _repository.CreateAsync(barberShop);
            return new ResponseModel { Success = result.Success};
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        try
        {
            var result = await _repository.DeleteAsync(id);
            return new ResponseModel { Success = result.Success };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseDataModel<IEnumerable<BarberShop>>> GetAllAsync()
    {
        try
        {
            var result = await _repository.GetAllAsync(1, 25);

            if (!result.Success)
                return new ResponseDataModel<IEnumerable<BarberShop>> { Success = false };
                
            return new ResponseDataModel<IEnumerable<BarberShop>> { Success = true, Data = result.Data };
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public async Task<IResponseDataModel<BarberShop>> GetByIdAsync(int id)
    {
        try
        {
            var result = await _repository.GetByIdAsync(id);

            if (!result.Success)
                return new ResponseDataModel<BarberShop> { Success = false };
                
            return new ResponseDataModel<BarberShop> { Success = true, Data = result.Data };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseModel> UpdateAsync(int id, BarberShopDtoRequest dto)
    {
        try
        {
            var result = await _repository.UpdateAsync(id, dto);
            return new ResponseModel { Success = result.Success };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
