using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class PersonService(IPersonRepository repository, IUserRepository userRepository)
    : BasePrimaryKeyService<Person, int>(repository), IPersonService
{
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<ISingleResponse<Person>> CreateAsync(IDtoRequest<Person> dtoRequest, int userId)
    {
        if (dtoRequest is not PersonDtoRegisterRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Person(dto, userId);
        var result = await CreateByEntityAsync(entity);

        if (!result.IsSuccess)
            return result;

        await _userRepository.AddUserRoleAsync(UserRole.Client);
        await _userRepository.UpdatePhoneNumberAsync(dto.PhoneNumber);

        return result;
    }

    public override async Task<IResponse> DeleteAsync(int id)
    {
        var resp = await GetByIdAsync(id);

        if (!resp.IsSuccess)
            return resp;
            
        await _userRepository.RemoveFromRoleAsync(UserRole.Client);
        return await _repository.DeleteAsync(resp.Value!);
    }
}
