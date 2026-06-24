using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AddressService(
    AppDbContext context,
    UserService userService,
    ILogger<AddressService> _logger)
    : BaseService<Address>(context)
{
    public async Task<AddressDtoResponse?> CreateAsync(
        AddressDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var address = new Address(dto, dto.BarberShopId);

        dbSet.Add(address);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;
        
        _logger.LogInformation("Address persisted in database with Id={Id}", address.Id);
        return address.CreateDto();
    }

    public async Task<EntityInfos> GetEntityInfosAsync(
        int id, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUserId = await userService.GetMyUserIdAsync();
        
        var infos = await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Select(a => new EntityInfos(
                !a.IsDeleted,
                currentUserId != null && a.BarberShopId == currentUserId,
                a.BarberShopId == barberShopId
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return infos ?? new();
    }
    
    public async Task<AddressDtoResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<AddressDtoResponse?> UpdateAsync(
        AddressDtoRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var address = await dbSet.FindAsync([id], cancellationToken);

        if (address is null)
            return null;
            
        _logger.LogDebug("Updating Address with Id={Id}", id);

        address.UpdateEntity(dto);

        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return address.CreateDto();
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var address = await dbSet.FindAsync([id], cancellationToken);

        if (address is null)
            return false;
        
        _logger.LogDebug("Deleting Address with Id={Id}", id);
        
        dbSet.Remove(address);
        return await SaveChangesAsync(cancellationToken);
    }
}
