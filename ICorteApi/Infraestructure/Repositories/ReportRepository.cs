using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ReportRepository(AppDbContext context, IReportErrors errors)
    : BaseRepository<Report>(context), IReportRepository
{
    private readonly IReportErrors _errors = errors;

    public override async Task<Report?> CreateAsync(Report report)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            var newReport = await CreateAsync(report);

            if (newReport is null)
                _errors.ThrowCreateException();

            await UpdateBarberShopRatingAsync(newReport!.BarberShop);

            await CommitAsync(transaction);
            return newReport;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public async Task<Report?> GetReportWithBarberShopByIdAsync(int id)
    {
        return await GetByIdAsync(x => x.Id == id, x => x.BarberShop);
    }

    public override async Task<bool> UpdateAsync(Report report)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            var result = await base.UpdateAsync(report);

            if (!result)
                _errors.ThrowUpdateException();

            await UpdateBarberShopRatingAsync(report.BarberShop);

            await CommitAsync(transaction);
            return result;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public override async Task<bool> DeleteAsync(Report report)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            var result = await base.DeleteAsync(report);

            if (!result)
                _errors.ThrowDeleteException();

            await UpdateBarberShopRatingAsync(report.BarberShop);

            await CommitAsync(transaction);
            return result;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    private async Task UpdateBarberShopRatingAsync(BarberShop barberShop)
    {
        var newRating = await _dbSet
            .Where(r => r.BarberShopId == barberShop.Id)
            .AverageAsync(r => (float)r.Rating);

        barberShop.UpdateRating(newRating);
    }
}
