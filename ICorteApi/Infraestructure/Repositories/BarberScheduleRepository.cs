using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberScheduleRepository(AppDbContext context, IUserRepository userRepository, IBarberShopErrors errors)
    : IBarberScheduleRepository
{
    private readonly AppDbContext _context = context;

    private readonly DbSet<BarberShop> _dbSetBarberShop = context.Set<BarberShop>();
    private readonly DbSet<Service> _dbSetService = context.Set<Service>();
    private readonly DbSet<Appointment> _dbSetAppointment = context.Set<Appointment>();
    private readonly DbSet<RecurringSchedule> _dbSetRecurringSchedule = context.Set<RecurringSchedule>();
    private readonly DbSet<SpecialSchedule> _dbSetSpecialSchedule = context.Set<SpecialSchedule>();

    private readonly IBarberShopErrors _errors = errors;

    private async Task<TimeSpan> CalculateTotalServiceDuration(int[] serviceIds)
    {
        var timeSpans = await _dbSetService
            .AsNoTracking()
            .Where(x => serviceIds.Contains(x.Id))
            .Select(x => new { x.Duration })
            .ToArrayAsync();
        
        return timeSpans.Aggregate(new TimeSpan(0), (acc, curr) => acc.Add(curr.Duration));
    }

    private async Task<Appointment[]> GetAppointmentsByDateAsync(int barberShopId, DateOnly date)
    {
        return await _dbSetAppointment
            .AsNoTracking()
            .Where(x => x.BarberShopId == barberShopId && x.Date == date)
            .OrderBy(x => x.StartTime)
            .ToArrayAsync();
    }

    private TimeOnly[] CalculateAvailableSlots(TimeOnly openTime, TimeOnly closeTime, Appointment[] appointments, TimeSpan serviceDuration)
    {
        var availableSlots = new List<TimeOnly>();

        // Ordena os compromissos pelo horário de início
        // var sortedAppointments = appointments.OrderBy(a => a.StartTime).ToList();

        // Define o primeiro slot disponível como o horário de abertura do barbeiro
        var currentTime = openTime;

        // Percorre todos os compromissos para calcular os slots disponíveis entre eles
        foreach (var appointment in appointments)
        {
            // Verifica se há espaço entre o horário atual e o início do próximo compromisso
            var nextAppointmentStartTime = appointment.StartTime;

            // Calcula a duração disponível entre o currentTime e o próximo compromisso
            if (nextAppointmentStartTime > currentTime)
            {
                var availableDuration = nextAppointmentStartTime - currentTime;

                // Se houver tempo suficiente para encaixar o serviço, adiciona o slot
                if (availableDuration >= serviceDuration)
                {
                    availableSlots.Add(currentTime);
                }
            }

            // Atualiza o horário atual para o fim do compromisso atual (adiciona a duração do serviço)
            currentTime = appointment.StartTime.Add(appointment.Services.Max(s => s.Duration)); // Supondo que Service tenha uma propriedade Duration
        }

        // Verifica se ainda há espaço entre o último compromisso e o horário de fechamento
        if (closeTime > currentTime)
        {
            var availableDuration = closeTime - currentTime;

            if (availableDuration >= serviceDuration)
            {
                availableSlots.Add(currentTime);
            }
        }

        return [..availableSlots];
    }

    public async Task<TimeOnly[]> GetAvailableSlotsAsync(int barberShopId, DateOnly date, int[] serviceIds)
    {
        var schedule = await _context.Database
            .SqlQuery<AvailableSchedule>(@$"
                SELECT {date} AS Date
                    ,COALESCE(SS.open_time, RS.open_time) AS OpenTime
                    ,COALESCE(SS.close_time, RS.close_time) AS CloseTime
                FROM recurring_schedules AS RS
                    LEFT JOIN special_schedules AS SS
                        ON SS.barber_shop_id = RS.barber_shop_id
                            AND RS.day_of_week = DATEPART(dw, SS.date) - 1
                            AND SS.is_active = 1
                WHERE RS.is_active = 1
                    AND RS.barber_shop_id = {barberShopId}
                    AND RS.day_of_week = {date.DayOfWeek}
                    AND (SS.is_closed IS NULL OR SS.is_closed <> 1)
            ")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (schedule is null)
            return [];
            
        var totalDuration = await CalculateTotalServiceDuration(serviceIds);
        var appointments = await GetAppointmentsByDateAsync(barberShopId, date);

        // Calcula os intervalos disponíveis
        return CalculateAvailableSlots(schedule.OpenTime, schedule.CloseTime, appointments, totalDuration);
    }

    public async Task<BarberShop[]> GetTopBarbersWithAvailabilityAsync(int weekNumber, int take = 10)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        return await _barberShopRep.GetTopBarbersWithAvailabilityAsync(startOfWeek.DayOfWeek, endOfWeek.DayOfWeek, take);
    }

    public async Task<DayOfWeek[]> GetAvailableDaysForBarberAsync(int barberShopId, int weekNumber)
    {
        var currentYear = DateTime.UtcNow.Year;
        var startOfWeek = ISOWeek.ToDateTime(currentYear, weekNumber, DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7);

        return await _recurringScheduleRep.GetAvailableDaysForBarberAsync(barberShopId, startOfWeek.DayOfWeek, endOfWeek.DayOfWeek);
    }

    private record AvailableSchedule(
        DateOnly Date,
        TimeOnly OpenTime,
        TimeOnly CloseTime
    );
}
