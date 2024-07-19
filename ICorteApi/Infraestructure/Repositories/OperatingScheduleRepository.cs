using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class RecurringScheduleRepository(AppDbContext context) : IRecurringScheduleRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public async Task<IResponseModel> CreateAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Add(recurringSchedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> CreateManyAsync(RecurringSchedule[] recurringSchedule)
    {
        _context.RecurringSchedules.AddRange(recurringSchedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var recurringSchedule = await _context.RecurringSchedules
            .SingleOrDefaultAsync(oh => oh.DayOfWeek == dayOfWeek && oh.BarberShopId == barberShopId);

        var response = new ResponseDataModel<RecurringSchedule>(
            recurringSchedule is not null,
            recurringSchedule
        );

        if (!response.Success)
            return response with { Message = "Horário de funcionamento não encontrado" };

        return response;
    }

    public async Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync(int barberShopId)
    {
        var recurringSchedules = await _context.RecurringSchedules
            .Where(oh => oh.BarberShopId == barberShopId).ToListAsync();

        return new ResponseDataModel<ICollection<RecurringSchedule>>(
            recurringSchedules is not null,
            recurringSchedules
        );
    }

    public async Task<IResponseModel> UpdateAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Update(recurringSchedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> UpdateManyAsync(RecurringSchedule[] recurringSchedule)
    {
        _context.RecurringSchedules.UpdateRange(recurringSchedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> DeleteAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Remove(recurringSchedule);
        return new ResponseModel(await SaveChangesAsync());
    }
}
