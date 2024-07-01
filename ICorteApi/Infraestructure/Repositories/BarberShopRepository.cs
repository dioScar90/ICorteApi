using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class BarberShopRepository(AppDbContext context) : IBarberShopRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<IResponseModel> CreateAsync(BarberShop barberShop)
    {
        try
        {
            await _context.BarberShops.AddAsync(barberShop);
            var newId = await _context.SaveChangesAsync();

            return new ResponseModel { Success = newId > 0 };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseDataModel<BarberShop>> GetAsync(Expression<Func<BarberShop, bool>> filter)
    {
        try
        {
            var barberShop = await _context.BarberShops.SingleOrDefaultAsync(filter);

            if (barberShop is null)
            {
                return new ResponseDataModel<BarberShop>
                {
                    Success = false,
                    Message = "BarberShop not found"
                };
            }

            return new ResponseDataModel<BarberShop>
            {
                Success = true,
                Data = barberShop
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<IResponseDataModel<IEnumerable<BarberShop>>> GetAllAsync(Expression<Func<BarberShop, bool>>? filter)
    {
        return new ResponseDataModel<IEnumerable<BarberShop>>
        {
            Success = true,
            Data = await _context.BarberShops.ToListAsync()
        };
    }

    public async Task<IResponseModel> UpdateAsync(BarberShop barberShop)
    {
        try
        {
            var barberShop = await context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);
            
            barberShop.Name = barberShop.Name;
            barberShop.Weight = barberShop.Weight;
            barberShop.Reps = barberShop.Reps;
            _context.BarberShops.Update(barberShop);
            return await _context.SaveChangesAsync() == 1 ?
              new ResponseModel { Success = true } :
              new ResponseModel
              {
                  Success = false
              };
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
            var liftData = await GetAsync(x => x.Id == id);
            if (!liftData.Success) return new ResponseModel { Success = false, Message = liftData.Message };
            _context.BarberShops.Remove(liftData.Data);
            return await _context.SaveChangesAsync() == 1 ?
                new ResponseModel { Success = true } :
                new ResponseModel
                {
                    Success = false
                };
        }
        catch (Exception)
        {

            throw;
        }
    }
}
