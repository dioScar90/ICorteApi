using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberShopRepository(AppDbContext context, IUserRepository userRepository, IBarberShopErrors errors)
    : BaseRepository<BarberShop>(context), IBarberShopRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IBarberShopErrors _errors = errors;
    
    public override async Task<BarberShop?> CreateAsync(BarberShop barberShop)
    {
        var transaction = await BeginTransactionAsync();

        try
        {
            var newbarberShop = await base.CreateAsync(barberShop);
            
            if (newbarberShop is null)
                _errors.ThrowCreateException();
            
            await _userRepository.AddUserRoleAsync(UserRole.BarberShop);
            
            await CommitAsync(transaction);
            return newbarberShop;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public override async Task<bool> DeleteAsync(BarberShop barberShop)
    {
        var transaction = await BeginTransactionAsync();

        try
        {
            var result = await base.DeleteAsync(barberShop);
            
            if (!result)
                _errors.ThrowDeleteException();

            await _userRepository.RemoveFromRoleAsync(UserRole.BarberShop);
            
            await CommitAsync(transaction);
            return result;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public async Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek, int take = 10)
    {
        return await _dbSet
            .Where(b => b.Reports.Any(r => r.Rating > 0))
            .OrderByDescending(b => b.Reports.Average(r => (decimal)r.Rating))
            .Select(b => new 
            {
                BarberShop = b,
                Availability = b.RecurringSchedules
                    .Where(s => s.DayOfWeek >= startDayOfWeek && s.DayOfWeek <= endDayOfWeek)
            })
            .Where(b => b.Availability.Any())
            .Take(take)
            .Select(b => b.BarberShop)
            .ToArrayAsync();
    }
}
