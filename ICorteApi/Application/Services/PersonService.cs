using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository, IUserService userService) : IPersonService
{
    private readonly IPersonRepository _repository = personRepository;
    private readonly IUserService _userService = userService;

    public async Task<IResponseModel> CreateAsync(Person person)
    {
        var result = await _repository.CreateAsync(person);
        return new ResponseModel { Success = result.Success};
    }

    public async Task<IResponseDataModel<BarberShop>> GetMyBarberShopAsync()
    {
        var myUserId = await _userService.GetUserIdAsync();

        if (myUserId is null)
            return new ResponseDataModel<BarberShop> { Success = false, Message = "Usuário não encontrado" };

        var responsePerson = await personRepository.GetByIdAsync((int)myUserId);

        if (!responsePerson.Success || responsePerson.Data.OwnedBarberShop is null)
            return new ResponseDataModel<BarberShop> { Success = false, Message = "Barbearia não encontrada" };

        return new ResponseDataModel<BarberShop> { Success = true, Data = responsePerson.Data.OwnedBarberShop };
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        return new ResponseModel { Success = result.Success };
    }

    public async Task<IResponseDataModel<IEnumerable<Person>>> GetAllAsync(int page, int pageSize)
    {
        var result = await _repository.GetAllAsync(1, 25);

        if (!result.Success)
            return new ResponseDataModel<IEnumerable<Person>> { Success = false };
            
        return new ResponseDataModel<IEnumerable<Person>> { Success = true, Data = result.Data };
    }
    
    public async Task<IResponseDataModel<Person>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);

        if (!result.Success)
            return new ResponseDataModel<Person> { Success = false, Message = "Não há ninguém com esse nome aqui" };
            
        return new ResponseDataModel<Person> { Success = true, Data = result.Data };
    }

    public async Task<IResponseModel> UpdateAsync(int id, PersonDtoRequest dto)
    {
        var result = await _repository.UpdateAsync(id, dto);
        return new ResponseModel { Success = result.Success };
    }
}
