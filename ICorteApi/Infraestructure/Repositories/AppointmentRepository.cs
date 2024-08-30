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
}
