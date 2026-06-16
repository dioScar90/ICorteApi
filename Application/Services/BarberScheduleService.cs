using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public class BarberScheduleService(AppDbContext context)
{
    private static int GetCorrectTakeNumber(int? take) => take is int and > 0 ? (int)take : 10;
    
    private record AvailableSchedule(TimeOnly OpenTime, TimeOnly CloseTime);
    
    private record BasicAppointment(TimeOnly StartTime, long ServicesDurationInTicks)
    {
        public TimeSpan ServicesDuration => TimeSpan.FromTicks(ServicesDurationInTicks);
    };
    
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
            currentTime = appointment.StartTime.Add(appointment.ServicesDuration);
        }
        
        // Verifica se há tempo disponível após o último appointment até o horário de fechamento
        while (currentTime.Add(serviceDuration) <= closeTime)
        {
            availableSlots.Add(currentTime);
            currentTime = currentTime.Add(serviceDuration);
        }
        
        return [..availableSlots];
    }

    private static (DateOnly, DateOnly) GetFirstAndLastDatesOfWeek(DateOnly randomDate)
    {
        DateOnly firstDateThisWeek = randomDate.AddDays(-(int)randomDate.DayOfWeek);
        DateOnly lastDateThisWeek = firstDateThisWeek.AddDays(6);

        return (firstDateThisWeek, lastDateThisWeek);
    }
    
    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        if (serviceIds.Length == 0)
            return [];
        
        var (firstDateThisWeek, _) = GetFirstAndLastDatesOfWeek(date);
        
        var availableSchedule = await context.RecurringSchedules
            .AsNoTracking()
            .LeftJoin(context.SpecialSchedules,
                rs => new { rs.BarberShopId, rs.DayOfWeek, Date = firstDateThisWeek.AddDays((int)rs.DayOfWeek) },
                ss => new { ss.BarberShopId, ss.DayOfWeek, ss.Date },
                (rs, ss) => new { rs, ss })
            .Where(x => x.rs.BarberShopId == barberShopId
                && x.rs.DayOfWeek == date.DayOfWeek
                && (x.ss == null || !x.ss.IsClosed))
            .Select(x => new AvailableSchedule(
                x.ss == null ? x.rs.OpenTime : (x.ss.OpenTime ?? x.rs.OpenTime),
                x.ss == null ? x.rs.CloseTime : (x.ss.CloseTime ?? x.rs.CloseTime)
            ))
            .FirstOrDefaultAsync();
            
        if (availableSchedule is null)
            return [];
            
        var totalDuration = TimeSpan.FromTicks(
            await context.Services
                .AsNoTracking()
                .Where(x => x.BarberShopId == barberShopId && serviceIds.Contains(x.Id))
                .SumAsync(x => x.Duration.Ticks)
        );
        
        if (totalDuration == TimeSpan.Zero)
            return [];
        
        var appointments = await context.Appointments
            .AsNoTracking()
            .Where(a => a.BarberShopId == barberShopId && a.Date == date)
            .Select(a => new BasicAppointment(
                a.StartTime,
                a.Services.Sum(s => s.Duration.Ticks)
            ))
            .ToArrayAsync();

        return CalculateAvailableSlots(availableSchedule.OpenTime, availableSchedule.CloseTime, appointments, totalDuration);
    }
    
    public async Task<TopBarberShopDtoResponse[]> GetTopBarbersWithAvailabilityAsync(DateOnly randomDate, int? _take)
    {
        var (firstDateThisWeek, lastDateThisWeek) = GetFirstAndLastDatesOfWeek(randomDate);

        int take = GetCorrectTakeNumber(_take);

        return await context.BarberShops
            .AsNoTracking()
            .Join(context.RecurringSchedules,
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

    public async Task<DateOnly[]> GetAvailableDatesForBarberAsync(int barberShopId, DateOnly randomDate)
    {
        var (firstDateThisWeek, _) = GetFirstAndLastDatesOfWeek(randomDate);
        
        return await context.RecurringSchedules
            .AsNoTracking()
            .GroupJoin(context.SpecialSchedules, // Left Join
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
    
    private static PaginationResponse<ServiceByNameDtoResponse> GetEmptyPagination() => new([], 0, 0, 1, 0);
    
    public async Task<PaginationResponse<ServiceByNameDtoResponse>> SearchServicesByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return GetEmptyPagination();
            
        string[] keywords = [..new HashSet<string>(name.SplitByWhitespaces())];

        if (keywords.Length == 0)
            return GetEmptyPagination();
            
        bool isPostgre = context.Database.ProviderName!.Contains("Postgre", StringComparison.InvariantCultureIgnoreCase);
        
        var services = await context.Services
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

        if (services is null)
            return GetEmptyPagination();
            
        return new PaginationResponse<ServiceByNameDtoResponse>(
            services,
            services.Length,
            1,
            1,
            services.Length
        );
    }
}
