using FluentValidation;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ReportService(
    AppDbContext context,
    UserService userService,
    ReportValidator validator,
    ReportErrors errors)
    : BaseService<Report, ReportDtoResponse, ReportDtoRequest>(context, userService)
{
    private readonly ReportValidator _validator = validator;
    private readonly ReportErrors _errors = errors;

    public override async Task<ReportDtoResponse> CreateAsync(ReportDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

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
        return await GetAllAsync(
            new(
                page,
                pageSize,
                x => x.BarberShopId == barberShopId,
                new(x => x.Id),
                    r => new ReportDtoResponse(
                    r.Id,
                    r.BarberShopId,
                    r.Title,
                    r.Content,
                    r.Rating
                )
            )
        );
    }
    
    public async Task<bool> UpdateAsync(ReportDtoRequest dto, int id, int barberShopId)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

        var report = await _dbSet.FindAsync(id);

        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        return await UpdateAsync(report, dto);
    }

    public async Task<bool> DeleteAsync(int id, int barberShopId)
    {
        var report = await _dbSet.FindAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();
            
        if (report!.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(report);
    }
}
