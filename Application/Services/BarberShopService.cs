using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(
    IBarberShopRepository repository,
    IValidator<BarberShopDtoCreate> createValidator,
    IValidator<BarberShopDtoUpdate> updateValidator,
    IBarberShopErrors errors)
    : BaseService<BarberShop>(repository), IBarberShopService
{
    private readonly IValidator<BarberShopDtoCreate> _createValidator = createValidator;
    private readonly IValidator<BarberShopDtoUpdate> _updateValidator = updateValidator;
    private readonly IBarberShopErrors _errors = errors;

    public async Task<BarberShopDtoResponse> CreateAsync(BarberShopDtoCreate dto, int ownerId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var barberShop = new BarberShop(dto, ownerId);
        return (await CreateAsync(barberShop))!.CreateDto();
    }
    
    public async Task<BarberShopDtoResponse> GetByIdAsync(int id)
    {
        var barberShop = await base.GetByIdAsync(id);

        if (barberShop is null)
            _errors.ThrowNotFoundException();
            
        return barberShop!.CreateDto();
    }
    
    public async Task<bool> UpdateAsync(BarberShopDtoUpdate dto, int id, int ownerId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var barberShop = await GetByIdAsync(x => x.Id == id, x => x.Address);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        barberShop!.UpdateEntityByDto(dto);
        return await UpdateAsync(barberShop);
    }

    public async Task<bool> DeleteAsync(int id, int ownerId)
    {
        var barberShop = await GetByIdAsync(x => x.Id == id, x => x.Address, x => x.RecurringSchedules, x => x.SpecialSchedules, x => x.Services);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        return await DeleteAsync(barberShop);
    }
}
