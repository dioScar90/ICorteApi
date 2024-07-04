using System.Linq.Expressions;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IOperatingScheduleRepository
{
    Task<IResponseModel> CreateAsync(OperatingSchedule operatingSchedule);
    Task<IResponseModel> CreateManyAsync(OperatingSchedule[] operatingSchedule);
    Task<IResponseDataModel<OperatingSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
    Task<IResponseDataModel<IEnumerable<OperatingSchedule>>> GetAllAsync(int barberShopId);
    Task<IResponseModel> UpdateAsync(OperatingSchedule operatingSchedule);
    Task<IResponseModel> UpdateManyAsync(OperatingSchedule[] operatingSchedule);
    Task<IResponseModel> DeleteAsync(OperatingSchedule operatingSchedule);
}
