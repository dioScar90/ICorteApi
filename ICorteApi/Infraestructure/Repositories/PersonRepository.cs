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
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseDataModel<Person>> GetByIdAsync(int userId)
    {
        var person = await _context.People.Include(p => p.OwnedBarberShop).SingleOrDefaultAsync(p => p.UserId == userId);
        var response = new ResponseDataModel<Person>(person is not null, person);

        if (!response.Success)
            return response with { Message = "Não há ninguém com esse nome aqui" };
            
        return response;
    }

    public async Task<IResponseDataModel<ICollection<Person>>> GetAllAsync(
        int page, int pageSize, Expression<Func<Person, bool>>? filter)
    {
        var people = await _context.People.ToListAsync();

        return new ResponseDataModel<ICollection<Person>>(
            people is not null,
            people
        );
    }

    public async Task<IResponseModel> UpdateAsync(Person person)
    {
        _context.People.Update(person);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> DeleteAsync(int userId)
    {
        var person = await _context.People.SingleOrDefaultAsync(p => p.UserId == userId);
        var response = new ResponseModel(person is not null);

        if (!response.Success)
            return response with { Message = "Usuário não encontrado" };

        _context.People.Remove(person!);
        return response with { Success = await SaveChangesAsync() };
    }
}
