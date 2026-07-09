using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ReportService(
    AppDbContext context,
    UserService userService)
    : BaseService<Report>(context)
{
    public async Task<ReportDtoResponse?> CreateAsync(
        ReportDtoRequest dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var clientId = await userService.GetMyUserIdAsync()!;
        
        if (clientId is null)
            return null;
            
        var report = new Report(dto, clientId.Value, dto.BarberShopId);
        
        dbSet.Add(report);
        
        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return report.CreateDto();
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
            .Select(r => new EntityInfos(
                r.DeletedAt == null,
                currentUserId != null && r.ClientId == currentUserId,
                r.BarberShopId == barberShopId
            ))
            .FirstOrDefaultAsync(cancellationToken);
            
        return infos ?? new();
    }
    
    public async Task<ReportDtoResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbSet
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new ReportDtoResponse(
                r.Id,
                r.BarberShopId,
                r.Title,
                r.Content,
                r.Rating
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<PaginationResponse<ReportDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetAllAsync<ReportDtoResponse>(
            new(
                page,
                pageSize,
                x => x.BarberShopId == barberShopId,
                new(x => x.Id),
                r => new(
                    r.Id,
                    r.BarberShopId,
                    r.Title,
                    r.Content,
                    r.Rating
                )
            ),
            cancellationToken
        );
    }
    
    public async Task<ReportDtoResponse?> UpdateAsync(
        ReportDtoRequest dto, int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var report = await dbSet.FindAsync([id], cancellationToken);

        if (report is null)
            return null;

        report.UpdateEntity(dto);

        if (!await SaveChangesAsync(cancellationToken))
            return null;

        return report.CreateDto();
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var report = await dbSet.FindAsync([id], cancellationToken);
        
        if (report is null)
            return false;
        
        dbSet.Remove(report);
        return await SaveChangesAsync(cancellationToken);
    }
}
