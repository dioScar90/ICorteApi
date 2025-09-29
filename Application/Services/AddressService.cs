using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AddressService(
    AppDbContext context,
    ILogger<AddressService> logger,
    AddressValidator validator,
    AddressErrors errors)
    : BaseService<Address>(context, logger)
{
    private readonly AddressValidator _validator = validator;
    private readonly AddressErrors _errors = errors;
    
    public async Task<AddressDtoResponse> CreateAsync(AddressDtoRequest dto)
    {
        _logger.LogDebug("Starting validation for Address {@Address}", dto);
        dto.ThrowExceptionIfInvalid(_validator, _errors, _logger);
        
        var address = new Address(dto, dto.BarberShopId);

        _dbSet.Add(address);
        await SaveChangesAsync();
        
        _logger.LogInformation("Address persisted in database with Id={Id}", address.Id);
        return await GetByIdAsync(address.Id, address.BarberShopId);
    }

    private async Task<Address> FindEntityAsync(int id, int barberShopId)
    {
        _logger.LogDebug("Fetching Address with Id={Id} from database", id);
        
        var address = await _dbSet.FindAsync(id);

        if (address is null)
            _errors.ThrowNotFoundException();

        if (address!.BarberShopId != barberShopId)
            _errors.ThrowAddressNotBelongsToBarberShopException(barberShopId);

        return address;
    }

    public async Task<AddressDtoResponse> GetByIdAsync(int id, int barberShopId)
    {
        _logger.LogDebug("Fetching Address with Id={Id} from database", id);

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
        _logger.LogDebug("Starting validation for Address {@Address}", dto);
        dto.ThrowExceptionIfInvalid(_validator, _errors, _logger);

        var address = await FindEntityAsync(id, dto.BarberShopId);

        _logger.LogDebug("Updating Address with Id={Id}", id);

        address.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var address = await FindEntityAsync(id, barberShopId);
        
        _logger.LogDebug("Deleting Address with Id={Id}", id);
        return await DeleteAsync(address);
    }
}
