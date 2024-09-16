using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberScheduleRepository(AppDbContext context) : IBarberScheduleRepository
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<Service> _dbSetService = context.Set<Service>();
    private readonly DbSet<Appointment> _dbSetAppointment = context.Set<Appointment>();

    private async Task<TimeSpan> CalculateTotalServiceDuration(int barberShopId, int[] serviceIds)
    {
        var timeSpans = await _dbSetService
            .AsNoTracking()
            .Where(x => x.BarberShopId == barberShopId && serviceIds.Contains(x.Id))
            .Select(x => x.Duration)
            .ToArrayAsync();

        return timeSpans.Aggregate(TimeSpan.Zero, (acc, curr) => acc.Add(curr));
    }

    private async Task<Appointment[]> GetAppointmentsByDateAsync(int barberShopId, DateOnly date)
    {
        return await _dbSetAppointment
            .AsNoTracking()
            .Where(x => x.BarberShopId == barberShopId && x.Date == date)
            .OrderBy(x => x.StartTime)
            .ToArrayAsync();
    }

    private static TimeOnly[] CalculateAvailableSlots(TimeOnly openTime, TimeOnly closeTime, Appointment[] appointments, TimeSpan serviceDuration)
    {
        List<TimeOnly> availableSlots = [];
        var currentTime = openTime;

        foreach (var appointment in appointments)
        {
            var nextAppointmentStartTime = appointment.StartTime;

            // Verifica se há tempo disponível antes do próximo appointment
            if (nextAppointmentStartTime > currentTime)
            {
                var availableDuration = nextAppointmentStartTime - currentTime;

                if (availableDuration >= serviceDuration)
                {
                    availableSlots.Add(currentTime);
                }
            }

            // Atualiza currentTime para o fim deste appointment (início + duração total do appointment)
            currentTime = appointment.Services.Aggregate(appointment.StartTime, (acc, curr) => acc.Add(curr.Duration));
        }

        // Verifica se há tempo disponível após o último appointment até o horário de fechamento
        if (closeTime > currentTime)
        {
            var availableDuration = closeTime - currentTime;

            if (availableDuration >= serviceDuration)
            {
                availableSlots.Add(currentTime);
            }
        }

        return [.. availableSlots];
    }

    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        var schedule = await _context.Database
            .SqlQuery<AvailableSchedule>(@$"
                SELECT TOP (1) {date} AS Date
                    ,COALESCE(SS.open_time, RS.open_time) AS OpenTime
                    ,COALESCE(SS.close_time, RS.close_time) AS CloseTime
                FROM recurring_schedules AS RS
                    LEFT JOIN special_schedules AS SS
                        ON SS.barber_shop_id = RS.barber_shop_id
                            AND RS.day_of_week = DATEPART(weekday, SS.date) - 1
                            AND SS.is_active = 1
                WHERE RS.is_active = 1
                    AND RS.barber_shop_id = {barberShopId}
                    AND RS.day_of_week = {date.DayOfWeek}
                    AND (SS.is_closed IS NULL OR SS.is_closed = 0)
            ")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (schedule is null)
            return [];

        var totalDuration = await CalculateTotalServiceDuration(barberShopId, serviceIds);
        var appointments = await GetAppointmentsByDateAsync(barberShopId, date);

        return CalculateAvailableSlots(schedule.OpenTime, schedule.CloseTime, appointments, totalDuration);
    }

    public async Task<TopBarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(DateOnly firstDateOfWeek, DateOnly lastDateOfWeek, int take)
    {
        return await _context.Database
            .SqlQuery<TopBarberShopDtoResponse>(@$"
                WITH available_days AS (
                    SELECT 
                        RS.barber_shop_id,
                        DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) AS available_date
                    FROM recurring_schedules AS RS
                    LEFT JOIN special_schedules AS SS
                        ON SS.barber_shop_id = RS.barber_shop_id
                        AND SS.is_active = 1
                        AND DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) = SS.date
                    WHERE RS.is_active = 1
                    AND (SS.is_closed IS NULL OR SS.is_closed = 0)
                    AND DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) BETWEEN {firstDateOfWeek} AND {lastDateOfWeek}
                    GROUP BY RS.barber_shop_id, RS.day_of_week, SS.open_time, SS.close_time
                )

                SELECT TOP ({take}) BS.id AS Id
                    ,BS.name AS Name
                    ,BS.description AS Description
                    ,BS.rating AS Rating
                FROM barber_shops AS BS
                WHERE EXISTS (
                    SELECT 1
                    FROM available_days AS AD
                    WHERE AD.barber_shop_id = BS.id
                )
                ORDER BY BS.rating DESC
            ")
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly firstDateOfWeek)
    {
        return await _context.Database
            // Using ORDER BY without TOP or OFFSET throws an Exception.
            .SqlQuery<AvailableSchedule>(@$"
                SELECT TOP (7) DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) AS Date
                    ,COALESCE(SS.open_time, RS.open_time) AS OpenTime
                    ,COALESCE(SS.close_time, RS.close_time) AS CloseTime
                FROM recurring_schedules AS RS
                    LEFT JOIN special_schedules AS SS
                        ON SS.barber_shop_id = RS.barber_shop_id
                            AND SS.is_active = 1
                            AND DATEADD(DAY, RS.day_of_week, {firstDateOfWeek}) = SS.date
                WHERE RS.is_active = 1
                    AND RS.barber_shop_id = {barberShopId}
                    AND (SS.is_closed IS NULL OR SS.is_closed = 0)
                ORDER BY 1
            ")
            .AsNoTracking()
            .Select(x => x.Date)
            .ToArrayAsync();
    }

    private record AvailableSchedule(
        DateOnly Date,
        TimeOnly OpenTime,
        TimeOnly CloseTime
    );
}
