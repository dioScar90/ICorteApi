using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AddressService(
    AppDbContext context,
    UserService userService,
    AddressValidator validator,
    AddressErrors errors)
    : BaseService<Address, AddressDtoResponse, AddressDtoRequest>(context, userService)
{
    private readonly AddressValidator _validator = validator;
    private readonly AddressErrors _errors = errors;
    
    public override async Task<AddressDtoResponse> CreateAsync(AddressDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

        var address = new Address(dto, dto.BarberShopId);

        _dbSet.Add(address);
        await SaveChangesAsync();

        return await GetByIdAsync(address.Id, address.BarberShopId);
    }

    public async Task<AddressDtoResponse> GetByIdAsync(int id, int barberShopId)
    {
        var address = await _dbSet
            .AsNoTracking()
            .Select(a => new AddressDtoResponse(
                a.Id,
                a.BarberShopId,
                a.Street,
                a.Number,
                a.Complement,
                a.Neighborhood,
                a.City,
                a.State,
                a.PostalCode,
                a.Country
            ))
            .Where(a => a.Id == id && a.BarberShopId == barberShopId)
            .FirstOrDefaultAsync();

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);

        return address;
    }
    
    public async Task<bool> UpdateAsync(AddressDtoRequest dto, int id)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

        var address = await _dbSet.FindAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != dto.BarberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(dto.BarberShopId);

        return await UpdateAsync(address, dto);
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var address = await _dbSet.FindAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(address);
    }
}
