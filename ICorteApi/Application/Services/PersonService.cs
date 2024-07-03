using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class PersonService(IPersonRepository personRepository) : IPersonService
{
    private readonly IPersonRepository _repository = personRepository;

    public async Task<IResponseModel> CreateAsync(Person person)
    {
        try
        {
            var result = await _repository.CreateAsync(person);
            return new ResponseModel { Success = result.Success};
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        try
        {
            var result = await _repository.DeleteAsync(id);
            return new ResponseModel { Success = result.Success };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseDataModel<IEnumerable<Person>>> GetAllAsync(int page, int pageSize)
    {
        try
        {
            var result = await _repository.GetAllAsync(1, 25);

            if (!result.Success)
                return new ResponseDataModel<IEnumerable<Person>> { Success = false };
                
            return new ResponseDataModel<IEnumerable<Person>> { Success = true, Data = result.Data };
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public async Task<IResponseDataModel<Person>> GetByIdAsync(int id)
    {
        try
        {
            var result = await _repository.GetByIdAsync(id);

            if (!result.Success)
                return new ResponseDataModel<Person> { Success = false, Message = "Não há ninguém com esse nome aqui" };
                
            return new ResponseDataModel<Person> { Success = true, Data = result.Data };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponseModel> UpdateAsync(int id, PersonDtoRequest dto)
    {
        try
        {
            var result = await _repository.UpdateAsync(id, dto);
            return new ResponseModel { Success = result.Success };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
