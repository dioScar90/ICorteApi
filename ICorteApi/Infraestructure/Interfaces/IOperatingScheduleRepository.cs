using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRecurringScheduleRepository
{
    Task<IResponseModel> CreateAsync(RecurringSchedule recurringSchedule);
    Task<IResponseModel> CreateManyAsync(RecurringSchedule[] recurringSchedule);
    Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
    Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync(int barberShopId);
    Task<IResponseModel> UpdateAsync(RecurringSchedule recurringSchedule);
    Task<IResponseModel> UpdateManyAsync(RecurringSchedule[] recurringSchedule);
    Task<IResponseModel> DeleteAsync(RecurringSchedule recurringSchedule);
}
