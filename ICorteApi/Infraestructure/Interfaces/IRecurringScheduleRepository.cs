using System.Linq.Expressions;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IRecurringScheduleRepository
{
    Task<IResponseModel> CreateAsync(RecurringSchedule schedule);
    Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(int id);
    Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync(int barberShopId, int barberId);
    Task<IResponseModel> UpdateAsync(RecurringSchedule schedule);
    Task<IResponseModel> DeleteAsync(int id);
}
