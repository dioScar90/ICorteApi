using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class PersonRepository(AppDbContext context) : IPersonRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    
    public async Task<IResponseModel> CreateAsync(Person person)
    {
        _context.People.Add(person);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }

    public async Task<IResponseDataModel<Person>> GetByIdAsync(int userId)
    {
        var person = await _context.People.SingleOrDefaultAsync(p => p.UserId == userId);

        if (person is null)
            return new ResponseDataModel<Person> { Success = false, Message = "Usuário não encontrado" };

        return new ResponseDataModel<Person>
        {
            Success = true,
            Data = person,
        };
    }

    public async Task<IResponseDataModel<IEnumerable<Person>>> GetAllAsync(
        int page, int pageSize, Expression<Func<Person, bool>>? filter)
    {
        return new ResponseDataModel<IEnumerable<Person>>
        {
            Success = true,
            Data = await _context.People.ToListAsync()
        };
    }

    public async Task<IResponseModel> UpdateAsync(int userId, PersonDtoRequest dto)
    {
        var person = await _context.People.SingleOrDefaultAsync(p => p.UserId == userId);

        if (person is null)
            return new ResponseModel { Success = false };

        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;
        
        person.UpdatedAt = DateTime.UtcNow;
        
        return new ResponseModel { Success = await SaveChangesAsync() };
    }

    public async Task<IResponseModel> DeleteAsync(int userId)
    {
        var person = await _context.People.SingleOrDefaultAsync(p => p.UserId == userId);

        if (person is null)
            return new ResponseModel { Success = false, Message = "Barbearia não encontrada" };
        
        _context.People.Remove(person);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }
}
