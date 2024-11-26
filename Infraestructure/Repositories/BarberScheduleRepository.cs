using Microsoft.EntityFrameworkCore;

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
    
    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly dateToSearchForSlots, DateOnly firstDateThisWeek, int[] serviceIds)
    {
        var schedule = await _context.RecurringSchedules
            .AsNoTracking()
            .GroupJoin(_context.SpecialSchedules, // Left Join
                rs => new { rs.BarberShopId, rs.DayOfWeek, Date = firstDateThisWeek.AddDays((int)rs.DayOfWeek) },
                ss => new { ss.BarberShopId, ss.DayOfWeek, ss.Date },
                (rs, ss) => new { rs, ss })
            .SelectMany(ssrs => ssrs.ss.DefaultIfEmpty(),
                (ssrs, ss) => new { ssrs.rs, ss }
            )
            .Where(x => x.rs.BarberShopId == barberShopId
                && x.rs.DayOfWeek == dateToSearchForSlots.DayOfWeek
                && (x.ss == null || !x.ss.IsClosed))
            .Select(x => new AvailableSchedule(
                dateToSearchForSlots,
                x.ss == null ? x.rs.OpenTime : x.ss.OpenTime ?? x.rs.OpenTime,
                x.ss == null ? x.rs.CloseTime : x.ss.CloseTime ?? x.rs.CloseTime
            ))
            .FirstOrDefaultAsync();
            
        if (schedule is null)
            return [];

        var totalDuration = await CalculateTotalServiceDuration(barberShopId, serviceIds);

        if (totalDuration == TimeSpan.Zero)
            return [];
        
        var appointments = await GetAppointmentsByDateAsync(barberShopId, dateToSearchForSlots);

        return CalculateAvailableSlots(schedule.OpenTime, schedule.CloseTime, appointments, totalDuration);
    }

    public async Task<TopBarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(DateOnly firstDateThisWeek, DateOnly lastDateThisWeek, int take)
    {
        return await _context.BarberShops
            .AsNoTracking()
            .Join(_context.RecurringSchedules,
                b => b.Id,
                rs => rs.BarberShopId,
                (b, rs) => new { b, rs })
            .Where(x => !x.b.SpecialSchedules.Any(
                    ss => ss.Date >= firstDateThisWeek && ss.Date <= lastDateThisWeek && ss.DayOfWeek == x.rs.DayOfWeek
                ) || x.b.SpecialSchedules.Any(
                    ss => ss.Date >= firstDateThisWeek && ss.Date <= lastDateThisWeek && ss.DayOfWeek == x.rs.DayOfWeek && !ss.IsClosed
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
    }

    public async Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly firstDateThisWeek)
    {
        return await _context.RecurringSchedules
            .AsNoTracking()
            .GroupJoin(_context.SpecialSchedules, // Left Join
                rs => new { rs.BarberShopId, rs.DayOfWeek, Date = firstDateThisWeek.AddDays((int)rs.DayOfWeek) },
                ss => new { ss.BarberShopId, ss.DayOfWeek, ss.Date },
                (rs, ss) => new { rs, ss })
            .SelectMany(ssrs => ssrs.ss.DefaultIfEmpty(),
                (ssrs, ss) => new { ssrs.rs, ss })
            .Where(x => x.rs.BarberShopId == barberShopId
                && (x.ss == null || !x.ss.IsClosed))
            .OrderBy(x => x.rs.DayOfWeek)
            .Select(x => firstDateThisWeek.AddDays((int)x.rs.DayOfWeek))
            .ToArrayAsync();
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
    
    public async Task<ServiceByNameDtoResponse[]> SearchServicesByName(string[] keywords)
    {
        bool isPostgre = _context.Database.ProviderName!.Contains("Postgre", StringComparison.InvariantCultureIgnoreCase);
        
        return await _context.Services
            .AsNoTracking()
            .Where(x => keywords.All(keyword => isPostgre
                ? EF.Functions.ILike(x.Name, "%" + keyword + "%")
                : EF.Functions.Like(x.Name, "%" + keyword + "%")))
            .Select(x => new ServiceByNameDtoResponse(
                x.Id,
                x.BarberShop.Id,
                x.BarberShop.Name,
                x.Name,
                x.Description,
                x.Price,
                x.Duration))
            .ToArrayAsync();
    }
}
