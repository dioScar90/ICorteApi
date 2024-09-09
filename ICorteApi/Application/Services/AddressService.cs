using FluentValidation;
using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class AddressService(
    IAddressRepository repository,
    IValidator<AddressDtoCreate> createValidator,
    IValidator<AddressDtoUpdate> updateValidator,
    IAddressErrors errors)
    : BaseService<Address>(repository), IAddressService
{
    private readonly IValidator<AddressDtoCreate> _createValidator = createValidator;
    private readonly IValidator<AddressDtoUpdate> _updateValidator = updateValidator;
    private readonly IAddressErrors _errors = errors;

    public async Task<AddressDtoResponse> CreateAsync(AddressDtoCreate dto, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var address = new Address(dto, barberShopId);
        return (await CreateAsync(address))!.CreateDto();
    }

    public async Task<AddressDtoResponse> GetByIdAsync(int id, int barberShopId)
    {
        var address = await GetByIdAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);

        return address.CreateDto();
    }

    public async Task<bool> UpdateAsync(AddressDtoUpdate dto, int id, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var address = await _repository.GetByIdAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);
        
        address!.UpdateEntityByDto(dto);
        return await UpdateAsync(address);
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var address = await _repository.GetByIdAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(address!);
    }
}
