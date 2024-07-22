using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
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

    public async Task<IResponse> CreateAsync(BarberShop barberShop)
    {
        _context.BarberShops.Add(barberShop);
        return await SaveChangesAsync()
            ? Response.Success()
            : Response.Failure(new Error("Nao", "Rolou"));
    }

    public async Task<ISingleResponse<BarberShop>> GetByIdAsync(int id)
    {
        var barberShop = await _context.BarberShops
            .Include(b => b.Address)
            .Include(b => b.RecurringSchedules)
            .SingleOrDefaultAsync(b => b.Id == id);

        if (barberShop is null)
            return Response.Failure<BarberShop>(Error.BarberShopNotFound);

        return Response.Success(barberShop);
    }

    public async Task<ISingleResponse<BarberShop>> GetMyBarberShopAsync(int ownerId)
    {
        var barberShop = await _context.BarberShops.SingleOrDefaultAsync(b => b.OwnerId == ownerId);
        return barberShop is null ? Response.Failure<BarberShop>(Error.BarberShopNotFound) : Response.Success(barberShop);
    }

    public async Task<ICollectionResponse<BarberShop>> GetAllAsync(
        int page, int pageSize, Expression<Func<BarberShop, bool>>? filter)
    {
        var barberShops = await _context.BarberShops.ToListAsync();

        if (barberShops is null)
            return Response.FailureCollection<BarberShop>(new Error("Found", "Nothing"));

        return Response.Success(barberShops);
        // return new ResponseDataModel<ICollection<BarberShop>>(
        //     true,
        //     await _context.BarberShops.ToListAsync()
        // );
    }


    public async Task<IResponse> UpdateAsync(BarberShop barberShop)
    {
        _context.BarberShops.Update(barberShop);
        return await SaveChangesAsync()
            ? Response.Success()
            : Response.Failure(new Error("Nao", "Rolou"));
    }

    public async Task<IResponse> DeleteAsync(int id)
    {
        var barberShop = await _context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);
        var response = new ResponseModel(barberShop is not null);

        if (!response.Success)
            return Response.Failure(new Error("Nao", "Rolou"));

        _context.BarberShops.Remove(barberShop!);
        return await SaveChangesAsync()
            ? Response.Success()
            : Response.Failure(new Error("Nao", "Rolou"));
    }
}
