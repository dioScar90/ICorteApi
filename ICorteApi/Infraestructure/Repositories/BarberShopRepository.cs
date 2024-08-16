using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberShopRepository(AppDbContext context, IUserRepository userRepository)
    : BasePrimaryKeyRepository<BarberShop, int>(context), IBarberShopRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public override async Task<ISingleResponse<BarberShop>> CreateAsync(BarberShop barberShop)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var barberShopResult = await base.CreateAsync(barberShop);
            
            if (!barberShopResult.IsSuccess)
                throw new Exception(barberShopResult.Error.Description);
                
            var roleResult = await _userRepository.AddUserRoleAsync(UserRole.BarberShop);
            
            if (!roleResult.IsSuccess)
                throw new Exception(roleResult.Error.Description);
            
            await transaction.CommitAsync();
            return Response.Success(barberShopResult.Value!);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<BarberShop>(new("TransactionError", ex.Message));
        }
    }

    public override async Task<IResponse> DeleteAsync(BarberShop barberShop)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var barberShopResult = await base.DeleteAsync(barberShop);
            
            if (!barberShopResult.IsSuccess)
                throw new Exception(barberShopResult.Error.Description);

            var roleResult = await _userRepository.RemoveFromRoleAsync(UserRole.BarberShop);
            
            if (!roleResult.IsSuccess)
                throw new Exception(roleResult.Error.Description);
                
            await transaction.CommitAsync();
            return Response.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<BarberShop>(new("TransactionError", ex.Message));
        }
    }
}
