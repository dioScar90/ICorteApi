using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class RecurringScheduleRepository(AppDbContext context) : IRecurringScheduleRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    
    public async Task<IResponse> CreateAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Add(recurringSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }
    
    public async Task<IResponse> CreateManyAsync(RecurringSchedule[] recurringSchedule)
    {
        _context.RecurringSchedules.AddRange(recurringSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.CreateError);
    }

    public async Task<ISingleResponse<RecurringSchedule>> GetByIdAsync(int barberShopId, DayOfWeek dayOfWeek)
    {
        var recurringSchedule = await _context.RecurringSchedules
            .AsNoTracking()
            .SingleOrDefaultAsync(rs => rs.BarberShopId == barberShopId && rs.DayOfWeek == dayOfWeek);

        if (recurringSchedule is null)
            return Response.Failure<RecurringSchedule>(Error.RecurringScheduleNotFound);

        return Response.Success(recurringSchedule);
    }
    
    public async Task<ICollectionResponse<RecurringSchedule>> GetAllAsync(int barberShopId)
    {
        var recurringSchedules = await _context.RecurringSchedules
            .AsNoTracking()
            .Where(rs => rs.BarberShopId == barberShopId)
            .ToListAsync();

        if (recurringSchedules is null)
            return Response.FailureCollection<RecurringSchedule>(Error.RecurringScheduleNotFound);

        return Response.Success(recurringSchedules);
    }
    
    public async Task<IResponse> UpdateAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Update(recurringSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }

    public async Task<IResponse> UpdateManyAsync(RecurringSchedule[] recurringSchedule)
    {
        _context.RecurringSchedules.UpdateRange(recurringSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.UpdateError);
    }
    
    public async Task<IResponse> DeleteAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Remove(recurringSchedule);
        return await SaveChangesAsync() ? Response.Success() : Response.Failure(Error.RemoveError);
    }
}
