using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public class OperatingScheduleRepository(AppDbContext context) : IOperatingScheduleRepository
{
    private readonly AppDbContext _context = context;

    private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public async Task<IResponseModel> CreateAsync(OperatingSchedule operatingSchedule)
    {
        _context.OperatingSchedules.Add(operatingSchedule);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }

    public async Task<IResponseModel> CreateManyAsync(OperatingSchedule[] operatingSchedule)
    {
        _context.OperatingSchedules.AddRange(operatingSchedule);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }

    public async Task<IResponseDataModel<OperatingSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var operatingSchedule = await _context.OperatingSchedules.SingleOrDefaultAsync(oh => oh.DayOfWeek == dayOfWeek && oh.BarberShopId == barberShopId);

        if (operatingSchedule is null)
            return new ResponseDataModel<OperatingSchedule> { Success = false, Message = "Horário de funcionamento não encontrado" };

        return new ResponseDataModel<OperatingSchedule>
        {
            Success = true,
            Data = operatingSchedule,
        };
    }

    public async Task<IResponseDataModel<IEnumerable<OperatingSchedule>>> GetAllAsync(int barberShopId)
    {
        return new ResponseDataModel<IEnumerable<OperatingSchedule>>
        {
            Success = true,
            Data = await _context.OperatingSchedules.Where(oh => oh.BarberShopId == barberShopId).ToListAsync()
        };
    }

    public async Task<IResponseModel> UpdateAsync(OperatingSchedule operatingSchedule)
    {
        _context.OperatingSchedules.Update(operatingSchedule);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }

    public async Task<IResponseModel> UpdateManyAsync(OperatingSchedule[] operatingSchedule)
    {
        _context.OperatingSchedules.UpdateRange(operatingSchedule);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }

    public async Task<IResponseModel> DeleteAsync(OperatingSchedule operatingSchedule)
    {
        _context.OperatingSchedules.Remove(operatingSchedule);
        return new ResponseModel { Success = await SaveChangesAsync() };
    }
}
