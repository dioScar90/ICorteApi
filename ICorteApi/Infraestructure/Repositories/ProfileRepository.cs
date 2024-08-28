using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ProfileRepository(AppDbContext context, IUserRepository userRepository)
    : BaseRepository<Profile>(context), IProfileRepository
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ISingleResponse<Profile>> CreateAsync(Profile profile, string phoneNumber)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var profileResult = await CreateAsync(profile);

            if (!profileResult.IsSuccess)
            {
                errors.AddRange(profileResult.Error!);
                throw new Exception();
            }

            var roleResult = await _userRepository.AddUserRoleAsync(UserRole.Client);

            if (!roleResult.IsSuccess)
            {
                errors.AddRange(roleResult.Error!);
                throw new Exception();
            }

            var phoneResult = await _userRepository.UpdatePhoneNumberAsync(phoneNumber);

            if (!phoneResult.IsSuccess)
            {
                errors.AddRange(phoneResult.Error!);
                throw new Exception();
            }

            await CommitAsync(transaction);
            return Response.Success(profileResult.Value!);
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<Profile>([..errors]);
    }

    public override async Task<IResponse> DeleteAsync(Profile profile)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var profileResult = await base.DeleteAsync(profile);

            if (!profileResult.IsSuccess)
            {
                errors.AddRange(profileResult.Error!);
                throw new Exception();
            }

            var roleResult = await _userRepository.RemoveFromRoleAsync(UserRole.Client);

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

        return Response.Failure<Profile>([..errors]);
    }
}
