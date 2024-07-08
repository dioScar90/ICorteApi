using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository, IUserService userService) : IPersonService
{
    private readonly IPersonRepository _repository = personRepository;
    private readonly IUserService _userService = userService;

    public async Task<IResponseModel> CreateAsync(Person person)
    {
        return await _repository.CreateAsync(person);
    }

    public async Task<IResponseDataModel<BarberShop>> GetMyBarberShopAsync()
    {
        var myUserId = await _userService.GetUserIdAsync();
        var response = new ResponseDataModel<BarberShop>(myUserId is not null);

        if (myUserId is null)
            return response with { Message = "Usuário não encontrado" };

        var personResponse = await _repository.GetByIdAsync((int)myUserId);

        if (!personResponse.Success || personResponse.Data!.OwnedBarberShop is null)
            return response with { Message = "Barbearia não encontrada" };

        return response with { Success = true, Data = personResponse.Data.OwnedBarberShop };
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IResponseDataModel<ICollection<Person>>> GetAllAsync(int page, int pageSize)
    {
        return await _repository.GetAllAsync(1, 25);
    }

    public async Task<IResponseDataModel<Person>> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IResponseModel> UpdateAsync(int id, PersonDtoRequest dto)
    {
        var response = await _repository.GetByIdAsync(id);

        if (!response.Success)
            return new ResponseModel(response.Success, "Barbearia não encontrada");

        var person = response.Data!;

        person.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(person);
    }
}
