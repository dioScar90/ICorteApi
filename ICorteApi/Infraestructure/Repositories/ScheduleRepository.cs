using System.Linq.Expressions;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class ScheduleRepository(AppDbContext context) : IScheduleRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public async Task<IResponseModel> CreateAsync(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseDataModel<Schedule>> GetByIdAsync(int id)
    {
        var schedule = await _context.Schedules.SingleOrDefaultAsync(b => b.Id == id);

        var response = new ResponseDataModel<Schedule>(
            schedule is not null,
            schedule
        );

        if (!response.Success)
            return response with { Message = "Agenda não encontrada" };

        return response;
    }

    public async Task<IResponseDataModel<ICollection<Schedule>>> GetAllAsync(
        int page, int pageSize, Expression<Func<Schedule, bool>>? filter)
    {
        return new ResponseDataModel<ICollection<Schedule>>(
            true,
            await _context.Schedules.ToListAsync()
        );
    }

    // public async Task<IResponseModel> UpdateAsync(int id, ScheduleDtoRequest dto)
    // {
    //     try
    //     {
    //         var schedule = dto.RecurringSchedules.Length > 0
    //             ? await _context.Schedules.Include(bs => bs.RecurringSchedules).SingleOrDefaultAsync(b => b.Id == id)
    //             : await _context.Schedules.SingleOrDefaultAsync(b => b.Id == id);

    //         if (schedule is null)
    //             return new ResponseModel { Success = false };

    //         schedule.UpdateEntityByDto(dto);
    //         return new ResponseModel { Success = await SaveChangesAsync() };
    //     }
    //     catch (Exception)
    //     {
    //         throw;
    //     }
    // }

    public async Task<IResponseModel> UpdateAsync(Schedule schedule)
    {
        _context.Schedules.Update(schedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        var schedule = await _context.Schedules.SingleOrDefaultAsync(b => b.Id == id);
        var response = new ResponseModel(schedule is not null);

        if (!response.Success)
            return response with { Message = "Agenda não encontrada" };

        _context.Schedules.Remove(schedule!);
        return response with { Success = await SaveChangesAsync() };
    }
}
