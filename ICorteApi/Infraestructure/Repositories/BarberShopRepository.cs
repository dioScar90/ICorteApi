using System.Globalization;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberShopRepository(AppDbContext context, IUserRepository userRepository)
    : BaseRepository<BarberShop>(context), IBarberShopRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public override async Task<ISingleResponse<BarberShop>> CreateAsync(BarberShop barberShop)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var barberShopResult = await base.CreateAsync(barberShop);
            
            if (!barberShopResult.IsSuccess)
            {
                errors.AddRange(barberShopResult.Error!);
                throw new Exception();
            }
                
            var roleResult = await _userRepository.AddUserRoleAsync(UserRole.BarberShop);
            
            if (!roleResult.IsSuccess)
            {
                errors.AddRange(roleResult.Error!);
                throw new Exception();
            }
            
            await transaction.CommitAsync();
            return Response.Success(barberShopResult.Value!);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<BarberShop>([..errors]);
    }

    public override async Task<IResponse> DeleteAsync(BarberShop barberShop)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var barberShopResult = await base.DeleteAsync(barberShop);
            
            if (!barberShopResult.IsSuccess)
            {
                errors.AddRange(barberShopResult.Error!);
                throw new Exception();
            }

            var roleResult = await _userRepository.RemoveFromRoleAsync(UserRole.BarberShop);
            
            if (!roleResult.IsSuccess)
            {
                errors.AddRange(roleResult.Error!);
                throw new Exception();
            }
                
            await transaction.CommitAsync();
            return Response.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<BarberShop>([..errors]);
    }
    
    public async Task<ICollectionResponse<BarberShop>> GetTop10BarbersWithAvailabilityAsync(int weekNumber)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        var top10Barbers = await _dbSet
            .Where(b => b.Reports.Any(r => r.Rating > 0))
            .OrderByDescending(b => b.Reports.Average(r => (int)r.Rating))
            .Select(b => new 
            {
                BarberShop = b,
                Availability = b.RecurringSchedules
                    .Where(s => s.OpenTime.HasValue && s.CloseTime.HasValue)
                    .Where(s => s.DayOfWeek >= startOfWeek.DayOfWeek && s.DayOfWeek <= endOfWeek.DayOfWeek)
            })
            .Where(b => b.Availability.Any())
            .Take(10)
            .Select(b => b.BarberShop)
            .ToListAsync();

        return Response.Success(top10Barbers);
    }
}
