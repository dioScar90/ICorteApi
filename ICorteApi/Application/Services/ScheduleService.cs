using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class ScheduleService(IScheduleRepository scheduleRepository) : IScheduleService
{
    private readonly IScheduleRepository _repository = scheduleRepository;

    public Task<IResponseModel> CreateAsync(ScheduleDtoRequest dto)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseModel> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public List<Schedule> GenerateSchedulesForWeek(int barberId, DateTime startDate)
    {
        var recurringSchedules = _context.RecurringSchedules
            .Where(rs => rs.BarberId == barberId)
            .ToList();

        var schedules = new List<Schedule>();

        for (int i = 0; i < 7; i++)
        {
            var date = startDate.AddDays(i);
            var dayOfWeek = date.DayOfWeek;

            var dailySchedules = recurringSchedules
                .Where(rs => rs.DayOfWeek == dayOfWeek)
                .Select(rs => new Schedule
                {
                    BarberId = barberId,
                    Date = date,
                    StartTime = rs.StartTime,
                    EndTime = rs.EndTime
                })
                .ToList();

            schedules.AddRange(dailySchedules);
        }

        return schedules;
    }

    public List<TimeSlot> GetAvailableTimeSlots(int barberId, DateTime date)
    {
        var recurringSchedules = _context.RecurringSchedules
            .Where(rs => rs.BarberId == barberId && rs.DayOfWeek == date.DayOfWeek)
            .ToList();

        var appointments = _context.Appointments
            .Where(a => a.BarberId == barberId && a.AppointmentDate.Date == date.Date)
            .ToList();

        var availableTimeSlots = new List<TimeSlot>();

        foreach (var schedule in recurringSchedules)
        {
            var startTime = schedule.StartTime;
            while (startTime < schedule.EndTime)
            {
                var endTime = startTime.Add(TimeSpan.FromMinutes(30)); // Assume 30 minutes per appointment

                if (!appointments.Any(a => a.StartTime < endTime && a.EndTime > startTime))
                {
                    availableTimeSlots.Add(new TimeSlot
                    {
                        StartTime = startTime,
                        EndTime = endTime
                    });
                }

                startTime = endTime;
            }
        }

        return availableTimeSlots;
    }

    public Task<IResponseDataModel<ICollection<Schedule>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IResponseDataModel<Schedule>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseModel> UpdateAsync(ScheduleDtoRequest dto)
    {
        throw new NotImplementedException();
    }
}
