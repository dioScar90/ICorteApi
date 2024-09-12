using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class AppointmentRepository(AppDbContext context)
    : BaseRepository<Appointment>(context), IAppointmentRepository
{
    public async Task<Appointment?> GetByIdWithServicesAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Services)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Appointment[]> GetAppointmentsByDateAsync(int barberShopId, DateOnly date)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.BarberShopId == barberShopId && x.Date == date)
            .OrderBy(x => x.Date)
            .ToArrayAsync() ?? [];
    }
}
