using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AddressService(
    AppDbContext context,
    IValidator<AddressDto> validator,
    IAddressErrors errors)
    : BaseService<Address>(context), IAddressService
{
    private readonly IValidator<AddressDto> _validator = validator;
    private readonly IAddressErrors _errors = errors;

    public async Task<AddressDto> CreateAsync(AddressDto dto, int barberShopId)
    {
        var address = new Address(dto, barberShopId);
        return (await CreateAsync(address))!.CreateDto();
    }

    public async Task<AddressDto> GetByIdAsync(int id, int barberShopId)
    {
        var address = await GetByIdAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);

        return address.CreateDto();
    }

    public async Task<bool> UpdateAsync(AddressDto dto, int id, int barberShopId)
    {
        var address = await _dbSet.FindAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);
        
        address!.UpdateEntityByDto(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var address = await _dbSet.FindAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(address!);
    }
}
