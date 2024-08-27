using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class AppointmentRepository(AppDbContext context)
    : BaseRepository<Appointment>(context), IAppointmentRepository
{

    public async Task<ISingleResponse<Appointment>> GetByIdWithServicesAsync(int id)
    {
        var appointment = await _dbSet.AsNoTracking()
            .Include(x => x.ServiceAppointments)
                .ThenInclude(sa => sa.Service)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (appointment is null)
            return Response.Failure<Appointment>(Error.TEntityNotFound);

        return Response.Success(appointment);
    }
}
