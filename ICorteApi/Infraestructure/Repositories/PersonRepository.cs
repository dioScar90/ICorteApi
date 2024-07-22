using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class PersonRepository(AppDbContext context) : IPersonRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    private async Task<IResponse> GetResponseBySaveChangesAsyn() =>
        await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.BadSave);

    public async Task<IResponse> CreateAsync(Person person)
    {
        _context.People.Add(person);
        return await GetResponseBySaveChangesAsyn();
    }

    public async Task<ISingleResponse<Person>> GetByIdAsync(int userId)
    {
        var person = await _context.People.Include(p => p.OwnedBarberShop).SingleOrDefaultAsync(p => p.UserId == userId);

        if (person is null)
            return Response.Failure<Person>(Error.PersonNotFound);
        
        return Response.Success(person);
    }

    public async Task<ICollectionResponse<Person>> GetAllAsync(
        int page, int pageSize, Expression<Func<Person, bool>>? filter)
    {
        var people = await _context.People
            .AsNoTracking()
            .ToListAsync();

        if (people is null)
            return Response.FailureCollection<Person>(Error.PersonNotFound);
        
        return Response.Success(people);
    }

    public async Task<IResponse> UpdateAsync(Person person)
    {
        _context.People.Update(person);
        return await GetResponseBySaveChangesAsyn();
    }

    public async Task<IResponse> DeleteAsync(int userId)
    {
        var person = await _context.People.SingleOrDefaultAsync(p => p.UserId == userId);

        if (person is null)
            return Response.Failure(Error.PersonNotFound);
        
        _context.People.Remove(person!);
        return await GetResponseBySaveChangesAsyn();
    }
}
