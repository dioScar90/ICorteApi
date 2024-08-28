using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ReportRepository(AppDbContext context, IBarberShopRepository barberShopRepository)
    : BaseRepository<Report>(context), IReportRepository
{
    private readonly IBarberShopRepository _barberShopRepository = barberShopRepository;

    public override async Task<ISingleResponse<Report>> CreateAsync(Report report)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var reportResult = await CreateAsync(report);

            if (!reportResult.IsSuccess)
            {
                errors.AddRange(reportResult.Error!);
                throw new Exception();
            }

            var ratingResult = await UpdateBarberShopRatingAsync(report.BarberShopId);

            if (!ratingResult.IsSuccess)
            {
                errors.AddRange(ratingResult.Error!);
                throw new Exception();
            }
            
            await CommitAsync(transaction);
            return Response.Success(reportResult.Value!);
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<Report>([..errors]);
    }

    public override async Task<IResponse> UpdateAsync(Report report)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var reportResult = await base.UpdateAsync(report);

            if (!reportResult.IsSuccess)
            {
                errors.AddRange(reportResult.Error!);
                throw new Exception();
            }

            var ratingResult = await UpdateBarberShopRatingAsync(report.BarberShopId);

            if (!ratingResult.IsSuccess)
            {
                errors.AddRange(ratingResult.Error!);
                throw new Exception();
            }

            await CommitAsync(transaction);
            return Response.Success();
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<Report>([..errors]);
    }

    public override async Task<IResponse> DeleteAsync(Report report)
    {
        var transaction = await BeginTransactionAsync();
        List<Error> errors = [];

        try
        {
            var reportResult = await base.DeleteAsync(report);

            if (!reportResult.IsSuccess)
            {
                errors.AddRange(reportResult.Error!);
                throw new Exception();
            }

            var ratingResult = await UpdateBarberShopRatingAsync(report.BarberShopId);

            if (!ratingResult.IsSuccess)
            {
                errors.AddRange(ratingResult.Error!);
                throw new Exception();
            }

            await CommitAsync(transaction);
            return Response.Success();
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction);

            if (errors.Count == 0)
                errors.Add(Error.TransactionError(ex.Message));
        }

        return Response.Failure<Report>([..errors]);
    }

    private async Task<IResponse> UpdateBarberShopRatingAsync(int barberShopId)
    {
        var resp = await _barberShopRepository.GetByIdAsync(barberShopId);

        if (!resp.IsSuccess)
            return resp;
        
        var barberShop = resp.Value!;

        var newRating = await _dbSet
            .Where(r => r.BarberShopId == barberShopId)
            .AverageAsync(r => (float)r.Rating);

        barberShop.UpdateRating(newRating);
        var respUpdateRating = await _barberShopRepository.UpdateAsync(barberShop);

        if (!respUpdateRating.IsSuccess)
            return respUpdateRating;

        return Response.Success();
    }
}
