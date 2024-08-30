using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberShopRepository(AppDbContext context, IUserRepository userRepository)
    : BaseRepository<BarberShop>(context), IBarberShopRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public override async Task<BarberShop?> CreateAsync(BarberShop barberShop)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var barberShopResult = await base.CreateAsync(barberShop);
            
            if (barberShopResult is null)
            {
                // errors.AddRange(barberShopResult);
                throw new Exception();
            }
            
            var roleResult = await _userRepository.AddUserRoleAsync(UserRole.BarberShop);
            
            if (!roleResult)
            {
                // errors.AddRange(roleResult.Error!);
                throw new Exception();
            }
            
            await CommitAsync(transaction);
            return barberShopResult;
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return null;
        // return Response.Failure<BarberShop>([..errors]);
    }

    public override async Task<bool> DeleteAsync(BarberShop barberShop)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var barberShopResult = await base.DeleteAsync(barberShop);
            
            if (!barberShopResult)
            {
                // errors.AddRange(barberShopResult.Error!);
                throw new Exception();
            }

            var roleResult = await _userRepository.RemoveFromRoleAsync(UserRole.BarberShop);
            
            if (!roleResult)
            {
                // errors.AddRange(roleResult.Error!);
                throw new Exception();
            }
                
            await CommitAsync(transaction);
            return true;
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return false;
        // return Response.Failure<BarberShop>([..errors]);
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
