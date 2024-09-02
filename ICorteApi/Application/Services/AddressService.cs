using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AddressService(IAddressRepository repository, IAddressErrors errors)
    : BaseService<Address>(repository), IAddressService
{
    private readonly IAddressErrors _errors = errors;

    public async Task<Address?> CreateAsync(AddressDtoCreate dto, int barberShopId)
    {
        var address = new Address(dto, barberShopId);
        return await CreateAsync(address);
    }

    public async Task<Address?> GetByIdAsync(int id, int barberShopId)
    {
        var address = await GetByIdAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);

        return address;
    }

    public async Task<bool> UpdateAsync(AddressDtoUpdate dto, int id, int barberShopId)
    {
        var address = await GetByIdAsync(id, barberShopId);

        address!.UpdateEntityByDto(dto);
        return await UpdateAsync(address);
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var address = await GetByIdAsync(id, barberShopId);
        return await DeleteAsync(address!);
    }
}
