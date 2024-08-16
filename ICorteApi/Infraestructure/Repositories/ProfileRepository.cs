using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ProfileRepository(AppDbContext context, IUserRepository userRepository)
    : BasePrimaryKeyRepository<Profile, int>(context), IProfileRepository
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ISingleResponse<Profile>> CreateAsync(Profile profile, string phoneNumber)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var profileResult = await CreateAsync(profile);

            if (!profileResult.IsSuccess)
                throw new Exception(profileResult.Error.Description);

            var roleResult = await _userRepository.AddUserRoleAsync(UserRole.Client);

            if (!roleResult.IsSuccess)
                throw new Exception(roleResult.Error.Description);

            var phoneResult = await _userRepository.UpdatePhoneNumberAsync(phoneNumber);

            if (!phoneResult.IsSuccess)
                throw new Exception(phoneResult.Error.Description);

            await transaction.CommitAsync();
            return Response.Success(profileResult.Value!);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<Profile>(new("TransactionError", ex.Message));
        }
    }

    public override async Task<IResponse> DeleteAsync(Profile profile)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var profileResult = await base.DeleteAsync(profile);

            if (!profileResult.IsSuccess)
                throw new Exception(profileResult.Error.Description);

            var roleResult = await _userRepository.RemoveFromRoleAsync(UserRole.Client);

            if (!roleResult.IsSuccess)
                throw new Exception(roleResult.Error.Description);

            await transaction.CommitAsync();
            return Response.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<Profile>(new("TransactionError", ex.Message));
        }
    }
}
