using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ProfileRepository(AppDbContext context, IUserRepository userRepository, IProfileErrors errors)
    : BaseRepository<Profile>(context), IProfileRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProfileErrors _errors = errors;

    public async Task<Profile?> CreateAsync(Profile profile, string phoneNumber)
    {
        var transaction = await BeginTransactionAsync();

        try
        {
            var newProfile = await CreateAsync(profile);

            if (newProfile is null)
                _errors.ThrowCreateException();

            await _userRepository.AddUserRoleAsync(UserRole.Client);
            await _userRepository.UpdatePhoneNumberAsync(phoneNumber);

            await CommitAsync(transaction);
            return newProfile;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public override async Task<bool> DeleteAsync(Profile profile)
    {
        var transaction = await BeginTransactionAsync();

        try
        {
            var result = await base.DeleteAsync(profile);

            if (!result)
                _errors.ThrowDeleteException();

            await _userRepository.RemoveFromRoleAsync(UserRole.Client);
            
            await CommitAsync(transaction);
            return result;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }
}
