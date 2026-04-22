using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AddressService(
    AppDbContext context,
    ILogger<AddressService> _logger)
    : BaseService<Address>(context)
{
    public async Task<AddressDtoResponse?> CreateAsync(AddressDtoRequest dto)
    {
        var address = new Address(dto, dto.BarberShopId);

        dbSet.Add(address);
        
        if (!await SaveChangesAsync())
            return null;
        
        _logger.LogInformation("Address persisted in database with Id={Id}", address.Id);
        return address.CreateDto();
    }
    
    private async Task<Address?> FindEntityAsync(int id, int barberShopId)
    {
        _logger.LogDebug("Fetching Address with Id={Id} from database", id);
        
        return await dbSet.FindAsync(id);
    }
    
    public async Task<AddressDtoResponse?> GetByIdAsync(int id, int barberShopId)
    {
        _logger.LogDebug("Fetching Address with Id={Id} from database", id);

        return await dbSet
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
    }
    
    public async Task<bool> UpdateAsync(AddressDtoRequest dto, int id)
    {
        var address = await FindEntityAsync(id, dto.BarberShopId);

        if (address is null)
            return false;
            
        _logger.LogDebug("Updating Address with Id={Id}", id);

        address.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var address = await FindEntityAsync(id, barberShopId);

        if (address is null)
            return false;
        
        _logger.LogDebug("Deleting Address with Id={Id}", id);
        
        dbSet.Remove(address);
        return await SaveChangesAsync();
    }
}
