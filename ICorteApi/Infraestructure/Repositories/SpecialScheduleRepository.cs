using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class SpecialScheduleRepository(AppDbContext context) : ISpecialScheduleRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    
    public async Task<IResponse> CreateAsync(SpecialSchedule specialSchedule)
    {
        _context.SpecialSchedules.Add(specialSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }
    
    public async Task<IResponse> CreateManyAsync(SpecialSchedule[] specialSchedules)
    {
        _context.SpecialSchedules.AddRange(specialSchedules);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }

    public async Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(int barberShopId, DateOnly date)
    {
        var specialSchedule = await _context.SpecialSchedules
            .AsNoTracking()
            .SingleOrDefaultAsync(ss => ss.BarberShopId == barberShopId && ss.Date == date);

        if (specialSchedule is null)
            return Response.Failure<SpecialSchedule>(Error.SpecialScheduleNotFound);

        return Response.Success(specialSchedule);
    }
    
    public async Task<ICollectionResponse<SpecialSchedule>> GetAllAsync(int barberShopId, DateOnly startDate, DateOnly endDate)
    {
        var specialSchedules = await _context.SpecialSchedules
            .AsNoTracking()
            .Where(ss => ss.BarberShopId == barberShopId && ss.Date >= startDate && ss.Date <= endDate)
            .ToListAsync();

        if (specialSchedules is null)
            return Response.FailureCollection<SpecialSchedule>(Error.SpecialScheduleNotFound);

        return Response.Success(specialSchedules);
    }
    
    public async Task<IResponse> UpdateAsync(SpecialSchedule specialSchedule)
    {
        _context.SpecialSchedules.Update(specialSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }

    public async Task<IResponse> UpdateManyAsync(SpecialSchedule[] specialSchedules)
    {
        _context.SpecialSchedules.UpdateRange(specialSchedules);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }
    
    public async Task<IResponse> DeleteAsync(SpecialSchedule specialSchedule)
    {
        _context.SpecialSchedules.Remove(specialSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
