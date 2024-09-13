using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ServiceRepository(AppDbContext context)
    : BaseRepository<Service>(context), IServiceRepository
{
    public async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        var hashIds = ids.ToHashSet();
        return await _dbSet.Where(x => hashIds.Contains(x.Id)).ToArrayAsync();
    }

    public async Task<bool> CheckCorrelatedAppointmentsAsync(int id)
    {
        return await _dbSet.AnyAsync(s => s.Id == id && s.Appointments.Any());
    }

    public async Task<Appointment[]> GetCorrelatedAppointmentsAsync(int id)
    {
        return await _dbSet
            .Where(s => s.Id == id)
            .SelectMany(s => s.Appointments)
            .ToArrayAsync();
    }
}
