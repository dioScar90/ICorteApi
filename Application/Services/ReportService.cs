using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ReportService(
    AppDbContext context,
    ILogger<ReportService> _logger,
    UserService _userService,
    ReportErrors _errors)
    : BaseService<Report>(context)
{
    public async Task<ReportDtoResponse> CreateAsync(ReportDtoRequest dto)
    {
        var clientId = await _userService.GetMyUserIdAsync()!;
        var report = new Report(dto, clientId, dto.BarberShopId);

        _dbSet.Add(report);
        await SaveChangesAsync();

        return await GetByIdAsync(report.Id, report.BarberShopId);
    }

    public async Task<ReportDtoResponse> GetByIdAsync(int id, int barberShopId)
    {
        var report = await _dbSet
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
        
        if (report is null)
            _errors.ThrowNotFoundException();
            
        if (report!.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        return report;
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
    
    public async Task UpdateAsync(ReportDtoRequest dto, int id, int barberShopId)
    {
        var report = await _dbSet.FindAsync(id);

        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        report.UpdateEntity(dto);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int barberShopId)
    {
        var report = await _dbSet.FindAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();
            
        if (report!.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);
        
        await DeleteAsync(report);
    }
}
