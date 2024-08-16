using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class PersonRepository(AppDbContext context, IUserRepository userRepository)
    : BasePrimaryKeyRepository<Person, int>(context), IPersonRepository
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ISingleResponse<Person>> CreateAsync(Person person, string phoneNumber)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var personResult = await CreateAsync(person);
            
            if (!personResult.IsSuccess)
                throw new Exception(personResult.Error.Description);

            var roleResult = await _userRepository.AddUserRoleAsync(UserRole.Client);
            
            if (!roleResult.IsSuccess)
                throw new Exception(roleResult.Error.Description);

            var phoneResult = await _userRepository.UpdatePhoneNumberAsync(phoneNumber);
            
            if (!phoneResult.IsSuccess)
                throw new Exception(phoneResult.Error.Description);

            await transaction.CommitAsync();
            return Response.Success(personResult.Value!);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<Person>(new("TransactionError", ex.Message));
        }
    }

    public override async Task<IResponse> DeleteAsync(Person person)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var personResult = await base.DeleteAsync(person);
            
            if (!personResult.IsSuccess)
                throw new Exception(personResult.Error.Description);

            var roleResult = await _userRepository.RemoveFromRoleAsync(UserRole.Client);
            
            if (!roleResult.IsSuccess)
                throw new Exception(roleResult.Error.Description);
                
            await transaction.CommitAsync();
            return Response.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Response.Failure<Person>(new("TransactionError", ex.Message));
        }
    }
}
