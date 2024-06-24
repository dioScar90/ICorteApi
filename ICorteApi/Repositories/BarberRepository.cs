using ICorteApi.Context;
using ICorteApi.Dtos;
using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Repositories;

public class BarberRepository(ICorteContext context) : BaseRepository(context), IBaseRepository
{
    private readonly ICorteContext _context = context;
    public IQueryable<Barber> GetAll(int page, int perPage)
    {
        page = page < 1 ? 1 : page;
        perPage = perPage < 1 ? 25 : perPage;

        return _context.Barbers
            .Where(b => b.IsActive)
            .Skip((page - 1) * perPage)
            .Take(perPage);

        // return await _context.Barbers
        //     .Where(b => b.IsActive)
        //     // .Select(b => new BarberDtoResponse(
        //     //     b.Id,
        //     //     b.Name,
        //     //     new AddressDtoResponse(
        //     //         b.Address.Id,
        //     //         b.Address.StreetType,
        //     //         b.Address.Street,
        //     //         b.Address.Number,
        //     //         b.Address.Complement,
        //     //         b.Address.Neighborhood,
        //     //         b.Address.City,
        //     //         b.Address.State,
        //     //         b.Address.PostalCode,
        //     //         b.Address.Country
        //     //     )
        //     // ))
        //     .Skip((page - 1) * perPage)
        //     .Take(perPage);
        //     // .ToListAsync();
    }

    public async Task<Barber?> GetById(int id)
    {
        return await _context.Barbers
            .SingleOrDefaultAsync(b => b.IsActive && b.Id == id);
    }
}
