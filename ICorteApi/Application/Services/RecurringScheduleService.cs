using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class RecurringScheduleService(IRecurringScheduleRepository recurringScheduleRepository) : IRecurringScheduleService
{
    private readonly IRecurringScheduleRepository _repository = recurringScheduleRepository;

    public Task<IResponseModel> CreateAsync(RecurringScheduleDtoRequest dto)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseModel> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async List<Schedule> GenerateRecurringSchedulesForWeek(int barberShopId, int barberId, DateTime startDate)
    {
        var response = await _repository.GetAllAsync(barberShopId, barberId);

        List<Schedule> schedules = [];
        var recurringSchedules = response.Data!;

        for (int i = 0; i < 7; i++)
        {
            var date = startDate.AddDays(i);
            var dayOfWeek = date.DayOfWeek;

            var dailyRecurringSchedules = recurringSchedules
                .Where(rs => rs.DayOfWeek == dayOfWeek)
                .Select(rs => new Schedule
                {
                    BarberId = barberId,
                    // Date = date,
                    StartTime = rs.StartTime,
                    EndTime = rs.EndTime
                })
                .ToList();

            schedules.AddRange(dailyRecurringSchedules);
        }

        return schedules;
    }

    public Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseModel> UpdateAsync(RecurringScheduleDtoRequest dto)
    {
        throw new NotImplementedException();
    }
}
