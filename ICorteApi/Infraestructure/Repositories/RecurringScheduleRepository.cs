using System.Globalization;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class RecurringScheduleRepository(AppDbContext context)
    : BaseRepository<RecurringSchedule>(context), IRecurringScheduleRepository
{
    public async Task<DayOfWeek[]> GetAvailableDaysForBarberAsync(int barberShopId, DayOfWeek startDayOfWeek, DayOfWeek endDayOfWeek)
    {
        return await _dbSet
            .Where(rs => rs.BarberShopId == barberShopId)
            .Where(rs => rs.DayOfWeek >= startDayOfWeek && rs.DayOfWeek <= endDayOfWeek)
            .Select(rs => rs.DayOfWeek)
            .Distinct()
            .ToArrayAsync();
    }
}
