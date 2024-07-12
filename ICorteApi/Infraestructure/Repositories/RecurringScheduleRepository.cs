using System.Linq.Expressions;
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

    public async Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(int id)
    {
        var recurringSchedule = await _context.RecurringSchedules.SingleOrDefaultAsync(b => b.Id == id);

        var response = new ResponseDataModel<RecurringSchedule>(
            recurringSchedule is not null,
            recurringSchedule
        );

        if (!response.Success)
            return response with { Message = "Agenda não encontrada" };
            
        return response;
    }

    public async Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync(int barberShopId, int barberId)
    {
        return new ResponseDataModel<ICollection<RecurringSchedule>>(
            true,
            await _context.RecurringSchedules
                .AsNoTrackingWithIdentityResolution()
                .Where(rs => rs.BarberShopId == barberShopId && rs.BarberId == barberId)
                .ToListAsync()
        );
    }

    // public async Task<IResponseModel> UpdateAsync(int id, RecurringScheduleDtoRequest dto)
    // {
    //     try
    //     {
    //         var recurringSchedule = dto.OperatingRecurringSchedules.Length > 0
    //             ? await _context.RecurringSchedules.Include(bs => bs.OperatingRecurringSchedules).SingleOrDefaultAsync(b => b.Id == id)
    //             : await _context.RecurringSchedules.SingleOrDefaultAsync(b => b.Id == id);

    //         if (recurringSchedule is null)
    //             return new ResponseModel { Success = false };

    //         recurringSchedule.UpdateEntityByDto(dto);
    //         return new ResponseModel { Success = await SaveChangesAsync() };
    //     }
    //     catch (Exception)
    //     {
    //         throw;
    //     }
    // }

    public async Task<IResponseModel> UpdateAsync(RecurringSchedule recurringSchedule)
    {
        _context.RecurringSchedules.Update(recurringSchedule);
        return new ResponseModel(await SaveChangesAsync());
    }

    public async Task<IResponseModel> DeleteAsync(int id)
    {
        var recurringSchedule = await _context.RecurringSchedules.SingleOrDefaultAsync(b => b.Id == id);
        var response = new ResponseModel(recurringSchedule is not null);

        if (!response.Success)
            return response with { Message = "Agenda não encontrada" };

        _context.RecurringSchedules.Remove(recurringSchedule!);
        return response with { Success = await SaveChangesAsync() };
    }
}
