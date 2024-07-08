using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class BarberShopRepository(AppDbContext context) : IBarberShopRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public async Task<IResponseModel> CreateAsync(BarberShop barberShop)
    {
        _context.BarberShops.Add(barberShop);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseDataModel<BarberShop>> GetByIdAsync(int id)
    {
        var barberShop = await _context.BarberShops
            .Include(b => b.Address)
            .Include(b => b.OperatingSchedules)
            .SingleOrDefaultAsync(b => b.Id == id);

         var response = new ResponseDataModel<BarberShop>(barberShop is not null, barberShop);

        if (response.Data is null)
            return response with { Message = "Barbeiro não encontrado" };
            
        return response;
    }

    public async Task<IResponseDataModel<ICollection<BarberShop>>> GetAllAsync(
        int page, int pageSize, Expression<Func<BarberShop, bool>>? filter)
    {
        return new ResponseDataModel<ICollection<BarberShop>>(
            true,
            await _context.BarberShops.ToListAsync()
        );
    }

    // public async Task<IResponseModel> UpdateAsync(int id, BarberShopDtoRequest dto)
    // {
    //     try
    //     {
    //         var barberShop = dto.OperatingSchedules.Length > 0
    //             ? await _context.BarberShops.Include(bs => bs.OperatingSchedules).SingleOrDefaultAsync(b => b.Id == id)
    //             : await _context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);

    //         if (barberShop is null)
    //             return new ResponseModel { Success = false };

    //         barberShop.UpdateEntityByDto(dto);
    //         return new ResponseModel { Success = await SaveChangesAsync() };
    //     }
    //     catch (Exception)
    //     {
    //         throw;
    //     }
    // }

    public async Task<IResponseModel> UpdateAsync(BarberShop barberShop)
    {
        _context.BarberShops.Update(barberShop);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        var barberShop = await _context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);
        var response = new ResponseModel(barberShop is not null);

        if (!response.Success)
            return response with { Message = "Barbearia não encontrada" };

        _context.BarberShops.Remove(barberShop!);
        return response with { Success = await SaveChangesAsync() };
    }
}
