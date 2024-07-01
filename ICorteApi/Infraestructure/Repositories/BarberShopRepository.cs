using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
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

    private async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
    
    public async Task<IResponseModel> CreateAsync(BarberShop barberShop)
    {
        try
        {
            await _context.BarberShops.AddAsync(barberShop);
            return new ResponseModel { Success = await SaveChangesAsync() };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseDataModel<BarberShop>> GetByIdAsync(Expression<Func<BarberShop, bool>> filter)
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

    public async Task<IResponseModel> UpdateAsync(int id, BarberShopDtoRequest dto)
    {
        try
        {
            var barberShop = await context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);

            if (barberShop is null)
                return new ResponseModel { Success = false };
            
            barberShop.Name = dto.Name;
            barberShop.Description = dto.Description;
            barberShop.PhoneNumber = dto.PhoneNumber;
            barberShop.ComercialNumber = dto.ComercialNumber;
            barberShop.ComercialEmail = dto.ComercialEmail;
            barberShop.OpeningHours = dto.OpeningHours;
            barberShop.ClosingHours = dto.ClosingHours;
            barberShop.DaysOpen = dto.DaysOpen;
            
            if (dto.Address is not null)
            {
                barberShop.Address.Street = dto.Address.Street;
                barberShop.Address.Number = dto.Address.Number;
                barberShop.Address.Complement = dto.Address.Complement;
                barberShop.Address.Neighborhood = dto.Address.Neighborhood;
                barberShop.Address.City = dto.Address.City;
                barberShop.Address.State = dto.Address.State;
                barberShop.Address.PostalCode = dto.Address.PostalCode;
                barberShop.Address.Country = dto.Address.Country;
            }

            barberShop.UpdatedAt = DateTime.UtcNow;
            
            return new ResponseModel { Success = await SaveChangesAsync() };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        var barberShop = await context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);

        if (barberShop is null)
            return new ResponseModel { Success = false, Message = "Barbearia não encontrada" };
        
        context.BarberShops.Remove(barberShop);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }
}
