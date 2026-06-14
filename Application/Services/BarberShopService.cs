using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(
    AppDbContext context,
    UserService userService)
    : BaseService<BarberShop>(context)
{
    public async Task<BarberShopDtoResponse?> CreateAsync(
        BarberShopDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ownerId = await userService.GetMyUserIdAsync()!;
        var barberShop = new BarberShop(dto, ownerId);

        dbSet.Add(barberShop);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return barberShop.CreateDto();
    }
    
    public async Task<EntityInfos> GetEntityInfosAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUserId = await userService.GetMyUserIdAsync();
        
        var infos = await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Select(b => new EntityInfos(
                !b.IsDeleted,
                currentUserId != null && b.OwnerId == currentUserId
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return infos ?? new();
    }
    
    public async Task<BarberShopDtoResponse?> GetByIdAsync(
        int id, bool withAddress = false, bool withOtherItems = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var query = dbSet
            .AsNoTracking()
            .Where(b => b.Id == id);

        if (withAddress)
        {
            query = query
                .AsSplitQuery()
                .Include(b => b.Address);
        }

        if (withOtherItems)
        {
            query = query
                .AsSplitQuery()
                .Include(b => b.RecurringSchedules)
                .Include(b => b.SpecialSchedules)
                .Include(b => b.Services)
                .Include(b => b.Reports);
        }

        return await query
            .Select(b => new BarberShopDtoResponse(
                b.Id,
                b.OwnerId,
                b.Name,
                b.Description,
                b.ComercialNumber,
                b.ComercialEmail,
                !withAddress ? null : new(
                    b.Address.Id,
                    b.Address.BarberShopId,
                    b.Address.Street,
                    b.Address.Number,
                    b.Address.Complement,
                    b.Address.Neighborhood,
                    b.Address.City,
                    b.Address.State,
                    b.Address.PostalCode,
                    b.Address.Country
                ),
                b.RecurringSchedules
                    .Where(_ => withOtherItems)
                    .Select(rs => new RecurringScheduleDtoResponse(
                        rs.DayOfWeek,
                        rs.BarberShopId,
                        rs.OpenTime,
                        rs.CloseTime,
                        rs.IsActive
                    )).ToArray(),
                b.SpecialSchedules
                    .Where(_ => withOtherItems)
                    .Select(ss => new SpecialScheduleDtoResponse(
                        ss.Date,
                        ss.BarberShopId,
                        ss.DayOfWeek,
                        ss.Notes,
                        ss.OpenTime,
                        ss.CloseTime,
                        ss.IsClosed
                    )).ToArray(),
                b.Services
                    .Where(_ => withOtherItems)
                    .Select(s => new ServiceDtoResponse(
                        s.Id,
                        s.BarberShopId,
                        s.BarberShop.Name,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    )).ToArray(),
                b.Reports
                    .Where(_ => withOtherItems)
                    .Select(r => new ReportDtoResponse(
                        r.Id,
                        r.BarberShopId,
                        r.Title,
                        r.Content,
                        r.Rating
                    )).ToArray()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PaginationResponse<AppointmentsByBarberShopDtoResponse>> GetAppointmentsByBarberShopAsync(
        int barberShopId, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ownerId = await userService.GetMyUserIdAsync()!;
        
        var query = context.Appointments
            .AsNoTracking()
            .AsSplitQuery()
            .Where(a => a.BarberShopId == barberShopId && a.BarberShop.OwnerId == ownerId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AppointmentsByBarberShopDtoResponse(
                a.Id,
                a.ClientId,
                new(
                    a.ClientId,
                    a.Client.Profile.FirstName,
                    a.Client.Profile.LastName,
                    a.Client.Profile.FirstName + ' ' + a.Client.Profile.LastName
                ),
                a.BarberShopId,
                a.Date,
                a.StartTime,
                a.TotalDuration,
                a.Notes,
                a.PaymentType,
                a.TotalPrice,
                a.Services.Select(s => new ServiceDtoResponse(
                    s.Id,
                    s.BarberShopId,
                    a.BarberShop.Name,
                    s.Name,
                    s.Description,
                    s.Price,
                    s.Duration
                )).ToArray(),
                a.Status
            ));

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        page = page > 0 && totalPages > 0 ? Math.Clamp(page, 1, totalPages) : 1;

        var entities = totalItems == 0 ? [] : await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new(entities ?? [], totalItems, totalPages, page, pageSize);
    }
    
    public async Task<bool> UpdateAsync(
        BarberShopDtoRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var barberShop = await dbSet.FindAsync([id], cancellationToken);

        if (barberShop is null)
            return false;
        
        barberShop.UpdateEntity(dto);
        return await SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var barberShop = await dbSet.FindAsync([id], cancellationToken);

        if (barberShop is null)
            return false;
            
        dbSet.Remove(barberShop);
        return await SaveChangesAsync(cancellationToken);
    }
}
