using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ReportService(
    AppDbContext context,
    UserService userService)
    : BaseService<Report>(context)
{
    public async Task<ReportDtoResponse?> CreateAsync(ReportDtoRequest dto, int? clientId = null)
    {
        clientId ??= await userService.GetMyUserIdAsync()!;
        
        if (clientId is null)
            return null;
            
        var report = new Report(dto, clientId.Value, dto.BarberShopId);
        
        dbSet.Add(report);
        
        if (!await SaveChangesAsync())
            return null;

        return report.CreateDto();
    }
    
    public async Task<bool> ReportExists(int id)
    {
        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }
    
    public async Task<bool> ReportBelongsToBarberShop(int id, int barberShopId)
    {
        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && x.BarberShopId == barberShopId);
    }
    
    public async Task<bool> ReportBelongsToClient(int id, int? clientId = null)
    {
        clientId ??= await userService.GetMyUserIdAsync();

        return await dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && x.ClientId == clientId);
    }

    public async Task<ReportDtoResponse?> GetByIdAsync(int id, int barberShopId)
    {
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
            .FirstOrDefaultAsync();
    }
    
    public async Task<PaginationResponse<ReportDtoResponse>> GetAllAsync(
        int? page, int? pageSize, int barberShopId)
    {
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
            )
        );
    }
    
    public async Task<bool> UpdateAsync(ReportDtoRequest dto, int id)
    {
        var report = await dbSet.FindAsync(id);

        if (report is null)
            return false;

        report.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var report = await dbSet.FindAsync(id);
        
        if (report is null)
            return false;
        
        dbSet.Remove(report);
        return await SaveChangesAsync();
    }
}
