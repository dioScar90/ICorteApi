using System.Globalization;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberShopRepository(AppDbContext context, IUserRepository userRepository)
    : BaseRepository<BarberShop>(context), IBarberShopRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public override async Task<ISingleResponse<BarberShop>> CreateAsync(BarberShop barberShop)
    {
        var transaction = await BeginTransactionAsync();
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
            
            await CommitAsync(transaction);
            return Response.Success(barberShopResult.Value!);
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<BarberShop>([..errors]);
    }

    public override async Task<IResponse> DeleteAsync(BarberShop barberShop)
    {
        var transaction = await BeginTransactionAsync();
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
                
            await CommitAsync(transaction);
            return Response.Success();
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<BarberShop>([..errors]);
    }

    public async Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek, int take = 10)
    {
        return await _dbSet
            .Where(b => b.Reports.Any(r => r.Rating > 0)) // Verifica se há ratings
            .OrderByDescending(b => b.Reports.Average(r => (decimal)r.Rating)) // Ordena pela média dos ratings
            .Select(b => new 
            {
                BarberShop = b,
                Availability = b.RecurringSchedules
                    .Where(s => s.DayOfWeek >= startDayOfWeek && s.DayOfWeek <= endDayOfWeek)
            })
            .Where(b => b.Availability.Any()) // Filtra barbearias com disponibilidade
            .Take(take) // Limita o número de resultados
            .Select(b => b.BarberShop) // Seleciona as barbearias
            .ToArrayAsync();
    }
}
