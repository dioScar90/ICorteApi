using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberScheduleRepository(AppDbContext context) : IBarberScheduleRepository
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<Service> _dbSetService = context.Set<Service>();
    private readonly DbSet<Appointment> _dbSetAppointment = context.Set<Appointment>();
    
    private async Task<TimeSpan> CalculateTotalServiceDuration(int barberShopId, int[] serviceIds)
    {
        var services = await _dbSetService
            .AsNoTracking()
            .Where(x => x.BarberShopId == barberShopId && serviceIds.Contains(x.Id))
            .Select(x => new ServiceDuration(x.Id, x.Duration))
            .ToArrayAsync();
            
        return services.Aggregate(TimeSpan.Zero, (acc, curr) => acc.Add(curr.Duration));
    }
    
    private async Task<BasicAppointment> GetNewAppointmentWithServiceDuration(int appointmentId, TimeOnly startTime)
    {
        var services = await _dbSetService
            .AsNoTracking()
            .Where(x => x.Appointments.Any(a => a.Id == appointmentId))
            .Select(x => new ServiceDuration(x.Id, x.Duration))
            .ToArrayAsync();

        return new(appointmentId, startTime, services);


        // var services = await _context.Database
        //     .SqlQuery<ServiceDuration>(@$"
        //         SELECT S.id AS Id
        //             ,S.duration AS Duration
        //         FROM services S
        //         WHERE S.is_deleted = 0
        //             AND EXISTS (
        //                 SELECT 1
        //                 FROM service_appointment SA
        //                 WHERE SA.service_id = S.id AND SA.appointment_id = {appointmentId}
        //             )
        //     ")
        //     .AsNoTracking()
        //     .ToArrayAsync();

        // return new(appointmentId, startTime, services);
    }

    private async Task<BasicAppointment[]> GetAppointmentsByDateAsync(int barberShopId, DateOnly date)
    {
        return await _dbSetAppointment
            .Include(a => a.Services)
            .Where(a => a.BarberShopId == barberShopId && a.Date == date)
            .Select(a => new BasicAppointment(
                a.Id,
                a.StartTime,
                a.Services.Select(s => new ServiceDuration(s.Id, s.Duration)).ToArray()
            ))
            .AsNoTracking()
            .ToArrayAsync();
    }

    private static TimeOnly[] CalculateAvailableSlots(TimeOnly openTime, TimeOnly closeTime, BasicAppointment[] appointments, TimeSpan serviceDuration)
    {
        List<TimeOnly> availableSlots = [];
        var currentTime = openTime;

        foreach (var appointment in appointments)
        {
            var nextAppointmentStartTime = appointment.StartTime;

            while (currentTime.Add(serviceDuration) <= nextAppointmentStartTime)
            {
                availableSlots.Add(currentTime);
                currentTime = currentTime.Add(serviceDuration);
            }
            
            // Atualiza currentTime para o fim deste appointment (início + duração total do appointment)
            currentTime = appointment.Services.Aggregate(appointment.StartTime, (acc, curr) => acc.Add(curr.Duration));
        }
        
        // Verifica se há tempo disponível após o último appointment até o horário de fechamento
        while (currentTime.Add(serviceDuration) <= closeTime)
        {
            availableSlots.Add(currentTime);
            currentTime = currentTime.Add(serviceDuration);
        }
        
        return [..availableSlots];
    }
    
    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        var schedule = await _context.RecurringSchedules
            .AsNoTracking()
            .GroupJoin(_context.SpecialSchedules, // Left Join
                rs => new { rs.BarberShopId, rs.DayOfWeek },
                ss => new { ss.BarberShopId, ss.DayOfWeek },
                (rs, ss) => new { rs, ss })
            .SelectMany(ssrs => ssrs.ss.DefaultIfEmpty(),
                (ssrs, ss) => new { ssrs.rs, ss }
            )
            .Where(x => x.rs.BarberShopId == barberShopId
                && x.rs.DayOfWeek == date.DayOfWeek
                && (x.ss == null || (x.ss.Date == date && !x.ss.IsClosed)))
            .Select(x => new AvailableSchedule(
                date,
                x.ss == null ? x.rs.OpenTime : x.ss.OpenTime ?? x.rs.OpenTime,
                x.ss == null ? x.rs.CloseTime : x.ss.CloseTime ?? x.rs.CloseTime
            ))
            .FirstOrDefaultAsync();
            
        if (schedule is null)
            return [];

        var totalDuration = await CalculateTotalServiceDuration(barberShopId, serviceIds);

        if (totalDuration == TimeSpan.Zero)
            return [];
        
        var appointments = await GetAppointmentsByDateAsync(barberShopId, date);

        return CalculateAvailableSlots(schedule.OpenTime, schedule.CloseTime, appointments, totalDuration);
    }

    public async Task<TopBarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(DateOnly firstDateOfWeek, DateOnly lastDateOfWeek, int take)
    {
        return await _context.BarberShops
            .AsNoTracking()
            .Join(_context.RecurringSchedules,
                b => b.Id,
                rs => rs.BarberShopId,
                (b, rs) => new { b, rs })
            .Where(x => !x.b.SpecialSchedules.Any(
                    ss => ss.Date >= firstDateOfWeek && ss.Date <= lastDateOfWeek && ss.DayOfWeek == x.rs.DayOfWeek
                ) || x.b.SpecialSchedules.Any(
                    ss => ss.Date >= firstDateOfWeek && ss.Date <= lastDateOfWeek && ss.DayOfWeek == x.rs.DayOfWeek && !ss.IsClosed
                ))
            .OrderByDescending(x => x.b.Rating)
                .ThenBy(x => x.b.Name)
            .Take(take)
            .Select(x => new TopBarberShopDtoResponse(
                x.b.Id,
                x.b.Name,
                x.b.Description,
                x.b.Rating
            ))
            .Distinct()
            .ToArrayAsync();
            
        // return await _context.Database
        //     .SqlQuery<TopBarberShopDtoResponse>(@$"
        //         WITH available_days AS (
        //             SELECT 
        //                 RS.barber_shop_id,
        //                 DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) AS available_date
        //             FROM recurring_schedules AS RS
        //             LEFT JOIN special_schedules AS SS
        //                 ON SS.barber_shop_id = RS.barber_shop_id
        //                 AND SS.is_active = 1
        //                 AND DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) = SS.date
        //             WHERE RS.is_active = 1
        //             AND (SS.is_closed IS NULL OR SS.is_closed = 0)
        //             AND DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) BETWEEN {firstDateOfWeek} AND {lastDateOfWeek}
        //             GROUP BY RS.barber_shop_id, RS.day_of_week, SS.open_time, SS.close_time
        //         )

        //         SELECT TOP ({take}) BS.id AS Id
        //             ,BS.name AS Name
        //             ,BS.description AS Description
        //             ,BS.rating AS Rating
        //         FROM barber_shops AS BS
        //         WHERE EXISTS (
        //             SELECT 1
        //             FROM available_days AS AD
        //             WHERE AD.barber_shop_id = BS.id
        //         )
        //         ORDER BY BS.rating DESC
        //     ")
        //     .AsNoTracking()
        //     .ToArrayAsync();
    }

    public async Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly firstDateOfWeek)
    {
        return await _context.RecurringSchedules
            .AsNoTracking()
            .GroupJoin(_context.SpecialSchedules, // Left Join
                rs => new { rs.BarberShopId, rs.DayOfWeek },
                ss => new { ss.BarberShopId, ss.DayOfWeek },
                (rs, ss) => new { rs, ss })
            .SelectMany(ssrs => ssrs.ss.DefaultIfEmpty(),
                (ssrs, ss) => new { ssrs.rs, ss })
            .Where(x => x.rs.BarberShopId == barberShopId
                && (x.ss == null || (x.ss.Date >= firstDateOfWeek && !x.ss.IsClosed)))
            .OrderBy(x => x.rs.DayOfWeek)
            .Select(x => firstDateOfWeek.AddDays((int)x.rs.DayOfWeek))
            .ToArrayAsync();

        // return await _context.Database
        //     // Using ORDER BY without TOP or OFFSET throws an Exception.
        //     .SqlQuery<AvailableSchedule>(@$"
        //         SELECT TOP (7) DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) AS Date
        //             ,COALESCE(SS.open_time, RS.open_time) AS OpenTime
        //             ,COALESCE(SS.close_time, RS.close_time) AS CloseTime
        //         FROM recurring_schedules AS RS
        //             LEFT JOIN special_schedules AS SS
        //                 ON SS.barber_shop_id = RS.barber_shop_id
        //                     AND SS.is_active = 1
        //                     AND DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) = SS.date
        //         WHERE RS.is_active = 1
        //             AND RS.barber_shop_id = {barberShopId}
        //             AND (SS.is_closed IS NULL OR SS.is_closed = 0)
        //         ORDER BY 1
        //     ")
        //     .AsNoTracking()
        //     .Select(x => x.Date)
        //     .ToArrayAsync();
    }

    private record ServiceDuration(
        int Id,
        TimeSpan Duration
    );
    
    private record BasicAppointment(
        int Id,
        TimeOnly StartTime,
        ServiceDuration[] Services
    );

    private record AvailableSchedule(
        DateOnly Date,
        TimeOnly OpenTime,
        TimeOnly CloseTime
    );
}
