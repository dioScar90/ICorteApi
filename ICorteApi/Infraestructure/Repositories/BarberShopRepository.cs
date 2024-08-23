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
}
